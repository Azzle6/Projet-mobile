using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceManager_LAC : MonoBehaviour
{
    
    public static RessourceManager_LAC instance { get; private set; }
    [Header("Ressources")]
    public float material, knowledge; 

    public ExtractorData extractorData = new ExtractorData();
    [Header("Select Extractor")]
    public LayerMask selectLayer;
    public Extractor_LAC selectedExtractor;
    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void Update()
    {
        // select extractor
        if(Input.touchCount >= 1)
        {
            Touch select = Input.GetTouch(0);
            if(select.phase == TouchPhase.Began)
            {
                RaycastHit hit;
                var ray = Camera.main.ScreenPointToRay(select.position);
                if(Physics.Raycast(ray, out hit,selectLayer))
                {
                    selectedExtractor = hit.transform.GetComponent<Extractor_LAC>();
                }
            }
        }
    }

    public void AddRessource(float value, Extractor_LAC.ProductType type)
    {
        if (type == Extractor_LAC.ProductType.MATERIAL)
            instance.material += value;

        if (type == Extractor_LAC.ProductType.KNOWLEDGE)
            instance.knowledge += value;

        extractorData.UpdateStock();   
    }
    [System.Serializable]
    public struct ExtractorData
    {
        public List<Extractor_LAC> extractors;
        public float globalProduct;
        public float ressource;

        public ExtractorData(float globalProduct = 0)
        {
            extractors = new List<Extractor_LAC>();
            this.globalProduct = globalProduct;
            ressource = 0;
        }

        public void AddExtractor(Extractor_LAC extractor)
        {
            if (extractors == null)
                extractors = new List<Extractor_LAC>();

            if (!extractors.Contains(extractor))
            {
                extractors.Add(extractor);
                globalProduct += extractor.productCapacity;

                if(extractor.productType == Extractor_LAC.ProductType.MATERIAL)
                     instance.material += extractor.stock;

                if (extractor.productType == Extractor_LAC.ProductType.KNOWLEDGE)
                    instance.knowledge += extractor.stock;
            }
        }

        public void RemoveExtractor(Extractor_LAC extractor)
        {
            if (extractors.Contains(extractor))
            {
                extractors.Remove(extractor);
                globalProduct -= extractor.productCapacity;

                if (extractor.productType == Extractor_LAC.ProductType.MATERIAL)
                    instance.material -= extractor.stock;

                if (extractor.productType == Extractor_LAC.ProductType.KNOWLEDGE)
                    instance.knowledge -= extractor.stock;
            }
        }
        public void UpdateGlobalProduct()
        {
            globalProduct = 0;
            for (int i = 0; i < extractors.Count; i++)
                globalProduct += extractors[i].productCapacity;
        }
        public void UpdateStock()
        {
            for (int i = 0; i < extractors.Count; i++)
                extractors[i].stock = instance.material*extractors[i].productCapacity / globalProduct;
        }

    }

}
