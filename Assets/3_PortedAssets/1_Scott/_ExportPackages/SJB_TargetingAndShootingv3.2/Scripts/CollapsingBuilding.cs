using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Scott.Barley.v2;

namespace Scott.Barley.v2
{
    /// <summary>
    /// 
    /// 
    /// 
    /// 
    /// Things to do: -
    ///     - turn of child parts after they are setup
    ///     - On Building Destory, 
    ///         - Turn off Sheel rendererer
    ///         - Turn on child parts
    ///     - after the building is destoryed deleted the game object
    ///     - Add a ShaderEffect for making the buildings look damaged
    ///     - Make the detonation points more variable
    ///     - Add Detonation Smoke Effect for when the building is collapsing
    /// </summary>
    public class CollapsingBuilding : MonoBehaviour
    {

 

        [Header("Set Up")]

        [SerializeField] GameObject buildingShell;       
        [SerializeField] LayerMask bulidingCollapseLayerMask;

        [Header("Building Collapse Settings")]
        // - Building Collapse     
        [SerializeField] float buildingCollapseTime = 2.5f;
        [SerializeField] float objectSegmentMass = 1000f;
        [SerializeField] float objectSegmentDrag = 0.0f;
        [Header("Building Collapse: Detonation Settings")]
        [SerializeField] GameObject detonationSmoke_PS_Prefab;
        [SerializeField] float explosiveForce = 55000f;
        [SerializeField] float upwardsForce = -8f;

        [Header("Damage Effects Settings")]
        // - Damage Indicators 
        [SerializeField] GameObject shellEffects_PS_Prefab;
        [SerializeField] float buildingBurnEffectTime = 5.5f;
        [SerializeField] int numberOfPoints = 12;
        [SerializeField] float minDistanceBetweenPoints = 10f;


        public List<Vector3> damageEffectPoint_List;
        public List<GameObject> fracturedBuildingParts_List;
        public List<GameObject> damageEffectsGO_List;
        
       
        private float _wait_timeToWaitTill_Collapse;
        private float _wait_collapseWaitPeriod;
        private bool collapseTriggered;
        private bool collapseCompleated;
        private int collapseIterationCount;

        private float _wait_timeToWaitTill_Effect;
        private float _wait_damageEffectsWaitPeriod;
        private bool damageEffectsTriggered;
        private bool damageEffectsCompleated;
        private int damageEffectsIterationCount;

        private bool meshSwitchFinnished;

        // variables for Physics.OverlapBox
        private Vector3 meshMidPoint;
        private Vector3 meshHalfExtents;


        //test varaible
        int testCount;

        private void Start()
        {
            
        }

        private void Awake()
        {
            buildingShell = this.transform.gameObject;
            bulidingCollapseLayerMask = LayerMask.GetMask("PhysicsLayer_BuildingCollapse");
            SetUp_BuildingCollapse();
            SetUp_DamageParticleEffectsPoints();
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
                Gizmos.DrawWireCube((transform.position + meshMidPoint), (meshHalfExtents*2));
        }

        #region Set Up Functions

        #region Set Up Functions -  Call Order of Set-Up Functions 
        void SetUp_DamageParticleEffectsPoints()
        {
            GenorateDammageEffectPoints();
            InstantiateAndPlaceEffectsPrefabAtEffectPointsAndSetActiveToFalse();
            OrderEffectsListBassedOnHeightFromTopToBottom();
            SetDamageEffectWaitPeriod();
            damageEffectsTriggered = false;
            damageEffectsCompleated = false;
            damageEffectsIterationCount = 0;
        }

        void SetUp_BuildingCollapse()
        {
            AddFracturedParts_Child_GameObjectsToList();
            AddBoxCollidersAndRigidBodies_AndDissabledThem();
            OrderListBassedOnHeightFromTopToBottom();
            SetCollapseWaitPeriod();
            collapseTriggered = false;
            collapseCompleated = false;
            collapseIterationCount = 0;
            GetModelBoundariesAndCenter_ByMinMax();
        }
        #endregion

        #region Set Up Functions - Building Collapse Functions
        void AddFracturedParts_Child_GameObjectsToList()
        {
            foreach (Transform childTransform in transform)
            {
                fracturedBuildingParts_List.Add(childTransform.gameObject);
            }
        }
        void AddBoxCollidersAndRigidBodies_AndDissabledThem()
        {
            foreach (GameObject buildingPart in fracturedBuildingParts_List)
            {
                buildingPart.AddComponent<BoxCollider>();
                Rigidbody rb = buildingPart.AddComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.mass = objectSegmentMass;
                rb.linearDamping = objectSegmentDrag;
            }
        }
        void OrderListBassedOnHeightFromTopToBottom()
        {
            fracturedBuildingParts_List = fracturedBuildingParts_List.OrderByDescending(go => go.transform.position.y).ToList();
        }

        void SetCollapseWaitPeriod()
        {
            _wait_collapseWaitPeriod = buildingCollapseTime / fracturedBuildingParts_List.Count;
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
            meshHalfExtents = new Vector3(Mathf.Abs(x_Max - x_Min)/2, Mathf.Abs(y_Max - y_Min)/2, Mathf.Abs(z_Max - z_Min) / 2);
            meshMidPoint = midPoint;
        }

        #endregion

        #region Set Up Functions - Damage Particle Effects

        private void GenorateDammageEffectPoints()
        {
            /// <summary>
            /// Builds a List of Points on the surface of the mesh, as based on  "numberOfPoints" & "minDistanceBetweenPoints"
            /// </summary>

            //int currentListPositionCheck = damageEffectPoint_List.Count - 1 >= 0 ? (damageEffectPoint_List.Count - 1) : 0;



            //if (setUPDammageEffectPointsCompleate == false){
                
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


            //    setUPDammageEffectPointsCompleate = true;
            //}
        }

        // Called in 'GenorateDammageEffectPoints'
        private Vector3 randomPointOnMesh()
        {

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

        private void InstantiateAndPlaceEffectsPrefabAtEffectPointsAndSetActiveToFalse()
        {

            foreach ( Vector3 effectPoint in damageEffectPoint_List)
            {
                GameObject gameObject = Instantiate(shellEffects_PS_Prefab, (transform.position + effectPoint), this.transform.rotation, this.transform);
                gameObject.SetActive(false);
                damageEffectsGO_List.Add(gameObject);
            }

        }


        void OrderEffectsListBassedOnHeightFromTopToBottom()
        {
            damageEffectsGO_List = damageEffectsGO_List.OrderByDescending(go => go.transform.position.y).ToList();

        }

        void SetDamageEffectWaitPeriod()
        {
            _wait_damageEffectsWaitPeriod = buildingBurnEffectTime / damageEffectsGO_List.Count;
        }


        // Not Used    --- may need to split up InstantiateAndPlaceEffectsPrefabAtEffectPointsAndSetActiveToFalse when pooling effect ---
        private void InstantiateDamageEffectsParticleSystemPrefabs()
        {
            // WARNING this may not be the same as the number of 
            for (int i = 0; i < numberOfPoints; i++)
            {
                GameObject gameObject = Instantiate(shellEffects_PS_Prefab);               
                damageEffectsGO_List.Add(gameObject);
                gameObject.SetActive(false);
            }
        }

        private void MoveDamageEffectsParticleSystemPrefabsToEffectLocations()
        {
            for (int i = 0; i < damageEffectsGO_List.Count; i++)
            {
                damageEffectsGO_List[i].transform.position = damageEffectPoint_List[i];
            }

        }


        #endregion

        #endregion

        #region Triggured State Functions

        #region Triggured State Functions - Primary Functions

        private void BuildingCollapse_ON_collapseTriggered()
        {
            if (collapseTriggered)
            {
                // switch from displaying the parent render, to showwing the children
                if (meshSwitchFinnished == false) SwitchFromShellToChildren();

                if (collapseCompleated == true) {
                    Destroy(this.transform.gameObject, 1f);
                }

                if (!collapseCompleated)
                {

                    if (_wait_timeToWaitTill_Collapse <= Time.time)
                    {
                        fracturedBuildingParts_List[collapseIterationCount].GetComponent<Rigidbody>().isKinematic = false;
                        collapseIterationCount++;
                        CollapseDetonations((collapseIterationCount*1f) / (fracturedBuildingParts_List.Count*1f));
                        _wait_Collapse();
                        if (collapseIterationCount >= fracturedBuildingParts_List.Count)
                        {
                            collapseCompleated = true;
                        }
                    }
                }
            }
        }

        private void DamageEffects_ON_damageEffectsTriggered()
        {
            if (damageEffectsTriggered)
            {
                if (!damageEffectsCompleated)
                {

                    if (_wait_timeToWaitTill_Effect <= Time.time)
                    {
                        damageEffectsGO_List[damageEffectsIterationCount].SetActive(true);
                        damageEffectsIterationCount++;
                        _wait_Effects();
                        if (damageEffectsIterationCount >= damageEffectsGO_List.Count)
                        {
                            damageEffectsCompleated = true;
                        }
                    }
                }
            }
        }

        #endregion

        #region Triggured State Functions - Sub Functions


        private void DisableDamageEffects()
        {
            // stops more damage effects being spawned 
            damageEffectsCompleated = true;

            foreach (GameObject go in damageEffectsGO_List)
            {
                go.SetActive(false);
            }

        }

        private void SwitchFromShellToChildren() {

            //Turn On Shatered Child Objects
            foreach (GameObject go in fracturedBuildingParts_List) {
                go.SetActive(true);
            }

            //Turn Off Rendere Of Parent
            this.transform.GetComponent<MeshRenderer>().enabled = false;
            meshSwitchFinnished = true;

        }

        void _wait_Collapse()
        {
            _wait_timeToWaitTill_Collapse = Time.time + _wait_collapseWaitPeriod;
        }
        void _wait_Effects()
        {
            _wait_timeToWaitTill_Effect = Time.time + _wait_damageEffectsWaitPeriod;
        }
        #endregion

        #endregion

        #region
        public void fnc_triggerDamageEffect() {
            damageEffectsTriggered = true;
        }

        public void fnc_triggerBuildingCollapse() {
            collapseTriggered = true;
        }


        #endregion


        // ------ Unfinished  -------

        private void CollapseDetonations(float lerpValue = 0.5f)
        {
            Collider[] collidersWithinBox = Physics.OverlapBox(transform.position + meshMidPoint, meshHalfExtents, this.transform.rotation, bulidingCollapseLayerMask);

            foreach (Collider col in collidersWithinBox)
            {

                // Code Author:  Mark Hoey

                Rigidbody rb = col.GetComponent<Rigidbody>();

                //If the object doesn't have a Rigidbody component 
                //or is the object exerting the explosive force 
                //then don't run other instructions in the foreach loop
                if (!rb)
                {
                    continue;
                }

                Vector3 a = transform.position + meshMidPoint + new Vector3(0, meshHalfExtents.y, 0);
                Vector3 b = transform.position + meshMidPoint - new Vector3(0, meshHalfExtents.y, 0);

                rb.AddExplosionForce(explosiveForce, Vector3.Lerp( a,b, lerpValue), (meshHalfExtents.x + meshHalfExtents.z)/2, upwardsForce);
            }
        }


        // TEST Functions

        private void Test_ByKeyDown()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {

                //CollapseDetonations();

                //Debug.Log("Mesh Center: " + GetCenterPoint_ByAveragingVertexPositions());
                //GetModelBoundariesAndCenter_ByMinMax();

                //collapseTriggered = true;
                //randomPointOnMesh();
                //testObject.transform.position = randomPointOnMesh();

                testCount++;

                if (testCount >= 1)
                {
                    damageEffectsTriggered = true;
                }
                if (testCount >= 2)
                {
                    collapseTriggered = true;
                    DisableDamageEffects();
                }


            }
        }

        private void Test_PlacePrefabsAtEffectPoints(Vector3 vec3)
        {
            GameObject gameObjectForTest = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            gameObjectForTest.transform.position = vec3;
            Renderer rendererForTest = gameObjectForTest.GetComponent<Renderer>();
            rendererForTest.material.SetColor("_Color", Color.red);
        }

        // OLD & NOT USED
        private Vector3 GetCenterPoint_ByAveragingVertexPositions()
        {
            /// <summary>
            /// Returns the mesh's center, By averaging the position of mesh points.
            /// </summary> 
            
            Mesh m = buildingShell.GetComponent<MeshFilter>().mesh;
            Vector3 center = Vector3.zero;
            foreach (Vector3 v in m.vertices)
                center += v;
            return center / m.vertexCount;
        }

        void AddSmoke()
        {
            /// NOT USED - See 'randomPointOnMesh() instead'
            

            Vector3 randomOnUnitSphere = Random.onUnitSphere * 100;
            Ray raySmokePlacement = new Ray(buildingShell.transform.position, randomOnUnitSphere);
            Debug.DrawRay(buildingShell.transform.position, randomOnUnitSphere, Color.white);

            RaycastHit hit;
            Physics.Raycast(buildingShell.transform.position, randomOnUnitSphere, out hit);
            Debug.Log("#### RayCastTest ##### , hit position = " + hit.point);
        }


    }
}


