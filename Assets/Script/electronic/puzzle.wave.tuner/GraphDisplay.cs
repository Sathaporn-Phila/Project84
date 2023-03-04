using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using UnityEngine.UI;
using Unity.Jobs;
using UnityEngine.Jobs;
using System.Linq;
 
public class GraphDisplay : MonoBehaviour
{
    public int pointDisplay = 100;
    public float phaseShift = 0;
    public float amplitude = 2;
    
    public NativeArray<Vector2> points;
    private RectTransform rectTransform;
    private GridLayoutGroup grid;
    public RectTransform[] rects;
    struct SineTransform : IJobParallelFor {
        public NativeArray<Vector2> points;
        public float xScale,amplitude,phaseShift;
        public void Execute(int i){
            points[i] = new Vector2(i*xScale,amplitude*Mathf.Sin(Mathf.PI/50 * i + phaseShift));  
        }
    }
    struct CurveFit : IJobParallelFor {
        public NativeArray<UIVertex> verts;
        public NativeArray<Vector2> points;
        public Color color;
        
        public void Execute(int i){ 
            
        }
    }
    private void Awake() {
        grid = this.GetComponent<GridLayoutGroup>();
        points = new NativeArray<Vector2>(pointDisplay,Allocator.Persistent);
        rects = new RectTransform[100];
        rectTransform = this.transform.Find("wave.origin").GetComponent<RectTransform>();
        for(int i=0;i<pointDisplay;i++){
            GameObject pos = new GameObject("point",typeof(Image));
            pos.transform.SetParent(rectTransform.transform);
            Debug.Log(pos.transform.localPosition);
            rects[i] = pos.GetComponent<RectTransform>();
            pos.GetComponent<Image>().color = Color.black;
        }

        float xScale = grid.cellSize.x/pointDisplay;

        SineTransform sineTransform = new(){
            points = points,
            xScale = xScale,
            amplitude = amplitude,
            phaseShift = phaseShift
        };
        
        JobHandle pointhandle = sineTransform.Schedule(points.Length,pointDisplay);  
        pointhandle.Complete();

        for(int i=0;i<pointDisplay;i++){
            rects[i].anchoredPosition3D = sineTransform.points[i];
            rects[i].sizeDelta = new Vector2(xScale,xScale);

        }

        points.Dispose();
        
    }
        

       
    
    
    
}
