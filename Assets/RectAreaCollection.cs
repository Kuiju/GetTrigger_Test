using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RectAreaCollection  {
    public List<RectArea > TriggerBorder = new List<RectArea >();
    public TargetAnchorConfig targetAnchorConfig;
    public bool IsPositionInTriggerArea(Vector2 position)
        {
        bool InTriggerArea = false;
        foreach (var border in TriggerBorder )
        {
            int nvert = border.BorderCornerList.Count;
            List<double> vertx= new List<double> ();
            List<double> verty = new List<double> ();
            for (int i = 0; i < nvert;i++ )
            {
                vertx.Add(border.BorderCornerList[i].x);
                verty.Add(border.BorderCornerList[i].y);
            }

            if( PositionPnpoly(nvert, vertx, verty, position.x, position.y))
                InTriggerArea =true ;

        }

            return InTriggerArea ;
        }

    public  bool PositionPnpoly(int nvert, List<double> vertx, List<double> verty, double testx, double testy)
    {
        int i, j, c = 0;
        for (i = 0, j = nvert - 1; i < nvert; j = i++)
        {
            if (((verty[i] > testy) != (verty[j] > testy)) && (testx < (vertx[j] - vertx[i]) * (testy - verty[i]) / (verty[j] - verty[i]) + vertx[i]))
            {
                c = 1 + c; 
            }
        }
        if (c % 2 == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }



}
