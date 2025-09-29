using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Scott.Barley.v2;

namespace Scott.Barley.v2
{

    public class DestructibleBuildingControler : MonoBehaviour
    {
        [Header("Set Up")]

        private DestructibleBuilding_ObjectPooling destructibleBuilding_ObjectPooling;
        [SerializeField] string buildingType_Tag;
        [SerializeField] string damageParticalEffect_Tag;


        [SerializeField] GameObject buildingShell;


        [Header("Damage Effects Settings")]
        // - Damage Indicators 
        [SerializeField] int numberOfPoints = 12;
        [SerializeField] float buildingBurnEffectTime = 5.5f;
        [SerializeField] float minDistanceBetweenPoints = 10f;

        public List<Vector3> damageEffectPoint_List;
        public List<DestructibleBuilding_ParticleSystemControl> DequeueParticalEffect_ControleScript_List;

        // --- Private Variables ---

        // - Damage Effects -
        private bool damageEffectsIsSetUp;
        private bool damageEffectsTriggered;
        private bool damageEffectsCompleated;
        private int damageEffectsIterationCount;
        private float _wait_timeToWaitTill_Effect;
        private float _wait_damageEffectsWaitPeriod;

        // - Building Collapse - 
        private float _wait_timeToWaitTill_Collapse;
        private float _wait_collapseWaitPeriod;
        private bool collapseTriggered;
        private bool collapseCompleated;
        private int collapseIterationCount;
        private bool collapseIsSetup;
        private GameObject dequedBuildingParts;
        private float _wait_timeToWaitTill_PostCollapseCleanUp;
        [SerializeField] float postCollapseCleanUpWaitTime;



        [Header("Building Collapse Settings")]

        [SerializeField] LayerMask bulidingCollapseLayerMask;
        [SerializeField] float explosiveForce = 55000f;
        [SerializeField] float upwardsForce = -1f;
        [SerializeField] List<GameObject> fracturedBuildingParts_List;  // building collapse parts list 


        // variables for Physics.OverlapSphere
        private Vector3 meshMidPoint;
        private Vector3 meshHalfExtents;

        [SerializeField] float buildingCollapseTime = 2.5f;


        private void RemoveParticalEffect()
        {
            foreach (DestructibleBuilding_ParticleSystemControl ps in DequeueParticalEffect_ControleScript_List)
            {
                ps.fnc_RetrunToPool();
            }
            DequeueParticalEffect_ControleScript_List.Clear();
        }





        private void Awake()
        {

        }


        private void Start()
        {
            collapseTriggered = false;
            damageEffectsTriggered = false;

            damageEffectsIsSetUp = false;
            collapseIsSetup = false;

        }
        private void Update()
        {
            //Test_ByKeyDown();
        }
        private void FixedUpdate()
        {
            BuildingCollapse_ON_collapseTriggered();
            DamageEffects_ON_damageEffectsTriggered();
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode

            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube((transform.position + meshMidPoint), (meshHalfExtents * 2));
        }









        private void BuildingCollapse_ON_collapseTriggered()
        {
            if (collapseTriggered)
            {
                if (collapseIsSetup == false)
                {
                    CollapseSetUp();
                    collapseIsSetup = true;
                }

                // turn off the buildings collider, once the collapse is triggered
                this.gameObject.GetComponent<Collider>().enabled = false;

                if (!collapseCompleated)
                {

                    if (_wait_timeToWaitTill_Collapse <= Time.time)
                    {

                        collapseIterationCount++;
                        CollapseDetonations((collapseIterationCount * 1f) / (fracturedBuildingParts_List.Count * 1f));
                        _wait_Collapse();
                        if (collapseIterationCount >= fracturedBuildingParts_List.Count)
                        {
                            collapseCompleated = true;
                            _wait_Collapse_PostCollapseCleanUp();
                        }
                    }
                    damageEffectsCompleated = true; // stops more particle effect from being spawned
                    
                }

                if (collapseCompleated == true)
                {
                    if(_wait_timeToWaitTill_PostCollapseCleanUp <= Time.time)
                    {
                        if(dequedBuildingParts.GetComponent<DestructibleBuilding_BuildingPartControl>() != null)
                        {
                            dequedBuildingParts.GetComponent<DestructibleBuilding_BuildingPartControl>().fnc_RetrunToPool();
                        }

                        Destroy(this.gameObject);

                    }
                }
            }
        }





        void SetCollapseWaitPeriod()
        {
            _wait_collapseWaitPeriod = buildingCollapseTime / fracturedBuildingParts_List.Count;
        }

        void _wait_Collapse()
        {
            _wait_timeToWaitTill_Collapse = Time.time + _wait_collapseWaitPeriod;
        }

        void _wait_Collapse_PostCollapseCleanUp()
        {
            _wait_timeToWaitTill_PostCollapseCleanUp = Time.time + postCollapseCleanUpWaitTime;
        }

        private void CollapseDetonations(float lerpValue = 0.5f)
        {
            Collider[] collidersWithinBox = Physics.OverlapBox(transform.position + meshMidPoint, meshHalfExtents, this.transform.rotation, bulidingCollapseLayerMask);

            //Collider[] collidersWithinSphere = Physics.OverlapSphere(transform.position + meshMidPoint, meshHalfExtents, bulidingCollapseLayerMask);

            foreach (Collider col in collidersWithinBox)
            {

                // Code Author:  Mark Hoey

                Rigidbody rb = col.GetComponent<Rigidbody>();
                //If the object doesn't have a Rigidbody component or is the object exerting the explosive force then don't run other instructions in the foreach loop
                if (!rb)
                {
                    continue;
                }

                Vector3 a = transform.position + meshMidPoint + new Vector3(0, meshHalfExtents.y, 0);
                Vector3 b = transform.position + meshMidPoint - new Vector3(0, meshHalfExtents.y, 0);

                rb.AddExplosionForce(explosiveForce, Vector3.Lerp(a, b, lerpValue), (meshHalfExtents.x + meshHalfExtents.z) / 2, upwardsForce);
            }
        }


        private void CollapseSetUp()
        {
            RemoveParticalEffect();
            DequeueBuildingParts_ByTag(buildingType_Tag, this.transform.position, this.transform.rotation);
            Destroy(buildingShell);
            GetModelBoundariesAndCenter_ByMinMax();
            SetCollapseWaitPeriod();

        }


        private void GetModelBoundariesAndCenter_ByMinMax()
        {
            Mesh m = buildingShell.GetComponent<MeshFilter>().mesh;
            List<Vector3> vList = new List<Vector3>();
            foreach (Vector3 v in m.vertices)
            {
                vList.Add(v);
            }

            vList = vList.OrderByDescending(go => go.x).ToList();

            float x_Max = vList[0].x;
            float x_Min = vList[vList.Count - 1].x;

            vList = vList.OrderByDescending(go => go.y).ToList();

            float y_Max = vList[0].y;
            float y_Min = vList[vList.Count - 1].y;

            vList = vList.OrderByDescending(go => go.z).ToList();

            float z_Max = vList[0].z;
            float z_Min = vList[vList.Count - 1].z;

            Vector3 meshBoundaryMax = new Vector3(x_Max, y_Max, z_Max);
            Vector3 meshBoundaryMin = new Vector3(x_Min, y_Min, z_Min);
            Vector3 midPoint = new Vector3(x_Max + (x_Min - x_Max) / 2, y_Max + (y_Min - y_Max) / 2, z_Max + (z_Min - z_Max) / 2);
            //Debug.Log("BoundaryMax, BoundaryMin, Mid Point: " + meshBoundaryMax + " , " + meshBoundaryMin + " , " + midPoint);
            //Debug.Log("LERP Test, Mid Point: " + Vector3.Lerp(meshBoundaryMax, meshBoundaryMin, 0.5f));
            meshHalfExtents = new Vector3(Mathf.Abs(x_Max - x_Min) / 2, Mathf.Abs(y_Max - y_Min) / 2, Mathf.Abs(z_Max - z_Min) / 2);
            meshMidPoint = midPoint;
        }



        public void DequeueBuildingParts_ByTag(string tag_PooledTagName, Vector3 position, Quaternion rotation)
        {
            if (destructibleBuilding_ObjectPooling != null)
            {
                GameObject projectileGO = destructibleBuilding_ObjectPooling.DequeueFromPool(tag_PooledTagName, position, rotation);

                dequedBuildingParts = projectileGO;
                foreach (Transform child in projectileGO.transform)
                {
                    fracturedBuildingParts_List.Add(child.gameObject);
                }

            }
            else
            {
                ConnectToObjectPoolInstance(); //if Not Connected, Connect!

                if (destructibleBuilding_ObjectPooling != null)
                {
                    GameObject projectileGO = destructibleBuilding_ObjectPooling.DequeueFromPool(tag_PooledTagName, position, rotation);
                    dequedBuildingParts = projectileGO;
                    foreach (Transform child in projectileGO.transform)
                    {
                        fracturedBuildingParts_List.Add(child.gameObject);
                    }
                }
            }
        }



        private void ConnectToObjectPoolInstance()
        {
            if (DestructibleBuilding_ObjectPooling.Instance != null)
            {
                destructibleBuilding_ObjectPooling = DestructibleBuilding_ObjectPooling.Instance; //this is causing endless issues!!!!   - somthing to do with execution order???
            }
        }


        #region --- Damage Effects: Related Functions ---
        public void DequeueParticalEffect_ByTag(string tag_PooledTagName, Vector3 position, Quaternion rotation)
        {
            if (destructibleBuilding_ObjectPooling != null)
            {
                GameObject projectileGO = destructibleBuilding_ObjectPooling.DequeueFromPool(tag_PooledTagName, position, rotation);
                if (projectileGO.GetComponent<DestructibleBuilding_ParticleSystemControl>() != null)
                {
                    DequeueParticalEffect_ControleScript_List.Add(projectileGO.GetComponent<DestructibleBuilding_ParticleSystemControl>());
                }


            }
            else
            {
                ConnectToObjectPoolInstance();
                if (destructibleBuilding_ObjectPooling != null)
                {
                    GameObject projectileGO = destructibleBuilding_ObjectPooling.DequeueFromPool(tag_PooledTagName, position, rotation);
                    if (projectileGO.GetComponent<DestructibleBuilding_ParticleSystemControl>() != null)
                    {
                        DequeueParticalEffect_ControleScript_List.Add(projectileGO.GetComponent<DestructibleBuilding_ParticleSystemControl>());
                    }
                }
            }

        }

        private void GenorateDammageEffectPoints()
        {
            /// <summary>
            /// Builds a List of Points on the surface of the mesh, as based on  "numberOfPoints" & "minDistanceBetweenPoints"
            /// </summary>


            bool tryAgainToFindPoint;
            int doWhileLoopTimeOutCount = 0;

            do
            {
                tryAgainToFindPoint = false;
                Vector3 pointOnMesh = randomPointOnMesh();
                foreach (Vector3 listVec3 in damageEffectPoint_List)
                {
                    if ((Vector3.Distance(pointOnMesh, listVec3) <= minDistanceBetweenPoints))
                    {
                        tryAgainToFindPoint = true;
                    }

                }
                if (tryAgainToFindPoint == false)
                {
                    damageEffectPoint_List.Add(pointOnMesh);
                }

                doWhileLoopTimeOutCount++;
                if (doWhileLoopTimeOutCount >= 499) { Debug.Log("SetUPDammageEffectPoints_ON_setUPDammageEffectPointsCompleate_False: Loop TimeOut"); }
            } while ((damageEffectPoint_List.Count < (numberOfPoints)) && doWhileLoopTimeOutCount < 500);

            Debug.Log("SetUPDammageEffectPoints_ON_setUPDammageEffectPointsCompleate_False : Finished : " + damageEffectPoint_List.Count + " Points added to List.");
        }
       
        private Vector3 randomPointOnMesh()
        {
            // Called in 'GenorateDammageEffectPoints'

            /// Method From: Konstantinos Vasileiadis
            /// Ref: https://www.youtube.com/watch?v=G5_ssRtKSEA

            Mesh buildingShellMesh = buildingShell.GetComponent<MeshFilter>().mesh;

            // Pick a random triangle (each triangle is 3 integers in a row in m.triangles)
            // So Pick a random origin (0, 3, 6, .. m.triangles.Length - 3)
            // -> Random (0.. m.triangles.Length / 3) * 3
            int triangleOrigin = Mathf.FloorToInt(Random.Range(0f, buildingShellMesh.triangles.Length) / 3f) * 3;
            Vector3 vertexA = buildingShellMesh.vertices[buildingShellMesh.triangles[triangleOrigin]];
            Vector3 vertexB = buildingShellMesh.vertices[buildingShellMesh.triangles[triangleOrigin + 1]];
            Vector3 vertexC = buildingShellMesh.vertices[buildingShellMesh.triangles[triangleOrigin + 2]];

            // Pick a random point on the triangle
            // For a uniform distribution, we pick randomly according to this:
            // http://mathworld.wolfram.com/TrianglePointPicking.html
            // From the point of origin (vertexA) move a random distance towards vertexB and from there a random distance in the direction of (vertexC - vertexB)
            // The only (temporary) downside is that we might end up with points outside our triangle as well, which have to be mapped back
            // The good thing is that these points can only end up in the triangle's "reflection" across the AC side (forming a quad AB, BC, CD, DA)

            // visual proof ref: http://www.leadinglesson.com/problem-on-geometric-proofs-with-vectors-1

            Vector3 dAB = vertexB - vertexA;
            Vector3 dBC = vertexC - vertexB;

            float rAB = Random.Range(0f, 1f);
            float rBC = Random.Range(0f, 1f);

            Vector3 randPoint = vertexA + rAB * dAB + rBC * dBC;

            // We have produces random points on a quad (the extension of our triangle)
            // To map back to the triangle, first we check if we are on the extension of the triangle
            // Since we can be on one of two triangles this is equivalent with checking if we are on the correct side of the AC line
            // If we are on the correct side (towards B) we are on the triangle - else we are not.

            // To check that we can compare the direction of our point towards any point on that line (say, C)
            // with the direction of the height of side AC (Cross (triangleNormal, dirBC)))
            Vector3 dirPC = (vertexC - randPoint).normalized;

            Vector3 dirAB = (vertexB - vertexA).normalized;
            Vector3 dirAC = (vertexC - vertexA).normalized;

            Vector3 triangleNormal = Vector3.Cross(dirAC, dirAB).normalized;

            Vector3 dirH_AC = Vector3.Cross(triangleNormal, dirAC).normalized;

            // If the two are alligned, we're in the wrong side
            float dot = Vector3.Dot(dirPC, dirH_AC);

            // We are on the right side, we're done
            if (dot >= 0)
            {
                // Otherwise, we need to find the symmetric to the center of the "quad" which is on the intersection of side AC with the bisecting line of angle (BA, BC)
                // Given by
                Vector3 centralPoint = (vertexA + vertexC) / 2;

                // And the symmetric point is given by the equation c - p = p_Sym - c => p_Sym = 2c - p
                Vector3 symmetricRandPoint = 2 * centralPoint - randPoint;

                //if (debugTransform) Debug.DrawLine(debugTransform.TransformPoint(randPoint), debugTransform.TransformPoint(symmetricRandPoint), Color.red, 10);
                randPoint = symmetricRandPoint;
            }
            return randPoint;


        }

        private void AlignEffectPointsWithBuildingTransform()
        {
            for (int i = 0; i < damageEffectPoint_List.Count; i++)
            {
                Vector3 alingedPosition = damageEffectPoint_List[i] + transform.position;
                damageEffectPoint_List[i] = alingedPosition;
            }
        }



        void _wait_Effects()
        {
            _wait_timeToWaitTill_Effect = Time.time + _wait_damageEffectsWaitPeriod;
        }

        void OrderEffectsListBassedOnHeightFromTopToBottom()
        {
            damageEffectPoint_List = damageEffectPoint_List.OrderByDescending(go => go.y).ToList();
        }

        void SetDamageEffectWaitPeriod()
        {
            _wait_damageEffectsWaitPeriod = buildingBurnEffectTime / damageEffectPoint_List.Count;
        }

        #endregion

        // --- Primary Trigger Functions ---
        #region Trigger Functions

        private void DamageEffects_ON_damageEffectsTriggered()
        {
            // If list of mesh spawn points have not already been generated, generate a list of points
            if (damageEffectsIsSetUp == false)
            {
                fnc_SetUp_DamageParticleEffectsPoints();
            }

            if (damageEffectsTriggered)
            {
                if (!damageEffectsCompleated)
                {

                    //Place the Effects, periodically so the building looks like it's catching fire
                    if (_wait_timeToWaitTill_Effect <= Time.time)
                    {
                        DequeueParticalEffect_ByTag(damageParticalEffect_Tag, damageEffectPoint_List[damageEffectsIterationCount], this.transform.rotation);
                        damageEffectsIterationCount++;
                        _wait_Effects();
                        if (damageEffectsIterationCount >= damageEffectPoint_List.Count)
                        {
                            damageEffectsCompleated = true;
                        }
                    }
                }
            }
        }



        #endregion





        // --- Public Functions ---
        #region Public Functions
        public void fnc_triggerDamageEffect()
        {
            damageEffectsTriggered = true;
        }

        public void fnc_triggerBuildingCollapse()
        {
            collapseTriggered = true;
        }


        /// <summary>
        /// Sets up 'Damage Partical Effects Points' based on the mesh of the object. Run when the building first takes damage, to save resources.
        /// </summary>
        public void fnc_SetUp_DamageParticleEffectsPoints()
        {
            if(damageEffectsIsSetUp == false)
            {
                GenorateDammageEffectPoints();
                AlignEffectPointsWithBuildingTransform();
                OrderEffectsListBassedOnHeightFromTopToBottom();
                SetDamageEffectWaitPeriod();
                damageEffectsTriggered = false;
                damageEffectsCompleated = false;
                damageEffectsIterationCount = 0;

                ConnectToObjectPoolInstance();

                damageEffectsIsSetUp = true;
            }
        }

        #endregion

        // --- Test Functions ---
        #region Test Functions
        //public List<GameObject> damageEffectsGO_List;
        private void Test()
        {
            fnc_SetUp_DamageParticleEffectsPoints();
        }
        private void Test_ByKeyDown()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                fnc_triggerDamageEffect();
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                fnc_triggerBuildingCollapse();
            }
        }
        #endregion

    }
}