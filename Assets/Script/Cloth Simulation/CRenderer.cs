using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRenderer
{
    public List<CCloth> Cloth_Model_list;
    public CRenderer()
    {
        Cloth_Model_list = new List<CCloth>();
        Cloth_Model_list.Clear();
    }
    public void AddClothModel(CCloth model)
    {
        Cloth_Model_list.Add(model);
    }
    public void update()
    {
        Cloth_Model_list[0].update();
    }
 
    public void render()
    {
        //for (int i = 0; i < Cloth_Model_list.Count; i++)
        //{
            Cloth_Model_list[0].render();
        //}
    }
}
