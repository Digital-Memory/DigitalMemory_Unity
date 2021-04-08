using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace performanceTest
{
    public class PerformanceTestHandler : MonoBehaviour
    {
        [SerializeField] Text intervallT, durationT, lGrowthT, mFactorT, createNewT, addMovementT;
        [SerializeField] GameObject UIgameObject;
        [SerializeField] Transform parent;
        [SerializeField] GameObject[] meshes;
        [SerializeField] Material[] materials;

        public PerformanceTest performanceTest;
        int counter = 0;
        float fps = float.MaxValue;
        public void RunTest()
        {
            counter = 0;
            fps = float.MaxValue;
            UIgameObject.SetActive(false);
            StartCoroutine(PerformaceTestRoutine(performanceTest));
        }

        public void ManualReset()
        {
            Destroy(parent.gameObject);
            parent = new GameObject("parent").transform;
        }

        IEnumerator PerformaceTestRoutine(PerformanceTest test)
        {
            float duration = test.duration;
            float amount = 1;

            while (fps > 30f)
            {
                for (int i = 0; i < amount; i++)
                {
                    GameObject instance = Instantiate(test.toDuplicate, new Vector3(UnityEngine.Random.Range(-10f, 10f), 0, UnityEngine.Random.Range(-10f, 10f)), Quaternion.identity, parent);

                    if (test.addMovementScript)
                        instance.AddComponent<PerformanceTestMovement>();

                    if (test.createNewMaterialInstance)
                    {
                        Material m = null;

                        foreach (MeshRenderer meshRenderer in instance.GetComponentsInChildren<MeshRenderer>())
                        {
                            if (m == null)
                                m = new Material(performanceTest.materialToUse);

                            meshRenderer.material = m;
                        }
                    }
                    else
                    {
                        foreach (MeshRenderer meshRenderer in instance.GetComponentsInChildren<MeshRenderer>())
                        {
                            meshRenderer.material = performanceTest.materialToUse;
                        }
                    }

                    counter++;
                }

                fps = FPS();
                duration -= test.intervall;
                amount += test.linearGrowthFactor;
                amount *= test.multiplicationFactor;
                Debug.Log(duration + " - " + amount + " ... " + fps);
                yield return new WaitForSeconds(test.intervall);
            }

            Debug.Log("reached " + fps + "FPS at counter " + counter);
            UIgameObject.SetActive(true);
        }

        private float FPS()
        {
            return 1.0f / Time.smoothDeltaTime;
        }

        private void OnGUI()
        {
            GUI.Box(new Rect(8, 8, 200, 100), counter.ToString() + " - " + fps + " \n" + (int)FPS());
            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void SetDuration(Single duration)
        {
            performanceTest.duration = duration;
            durationT.text = duration.ToString();
        }

        public void SetIntervall(Single intervall)
        {
            performanceTest.intervall = intervall;
            intervallT.text = intervall.ToString();
        }

        public void SetLinearGrowth(Single lGrowth)
        {
            performanceTest.linearGrowthFactor = lGrowth;
            lGrowthT.text = lGrowth.ToString();
        }

        public void SetMultiplicationFactor(Single mFactor)
        {
            performanceTest.multiplicationFactor = mFactor;
            mFactorT.text = mFactor.ToString();
        }

        public void SetCreateNewMaterialInstances(bool createNew)
        {
            performanceTest.createNewMaterialInstance = createNew;
        }

        public void SetAddMovementScript(bool addMovement)
        {
            performanceTest.addMovementScript = addMovement;
        }

        public void SetMesh(int meshId)
        {
            performanceTest.toDuplicate = meshes[meshId];
        }
        public void SetMaterials(int matId)
        {
            performanceTest.materialToUse = materials[matId];
        }
    }


    [System.Serializable]
    public class PerformanceTest
    {
        public float duration = 10f;
        public float intervall = 1f;
        public float linearGrowthFactor = 0f;
        public float multiplicationFactor = 1.1f;

        public bool createNewMaterialInstance = false;
        public bool addMovementScript = false;
        public GameObject toDuplicate;
        public Material materialToUse;
    }
}
