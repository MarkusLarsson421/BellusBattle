using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    [SerializeField] private int width = 20;
    [SerializeField] private int height = 10;
    [SerializeField] private float cellSize = 4;
    private MovableObject movableObject;
    Grid grid;

    public void Start()
    {
        grid = new Grid(width, height, cellSize, transform.position);
    }
    public void SetMovableObject(MovableObject mo)
    {
        movableObject = mo;
    }
    public void SS()
    {
        movableObject.transform.position = new Vector2(grid.GetX(movableObject.transform.position) * cellSize + transform.position.x + (cellSize * 0.5f), grid.GetY(movableObject.transform.position) * cellSize + transform.position.y + (cellSize * 0.5f));
        grid.SetValue(GetMaouseWorldPosition(), 1);
    }
    public void SS2(int w, int h)
    {
        float tempw = 0;
        float temph = 0;
        for(int i = 0; i < w; i++)
        {
            tempw += grid.GetX(movableObject.transform.position + new Vector3(cellSize * i, 0)) * cellSize + transform.position.x + (cellSize * 0.5f);
            grid.SetValue(GetMaouseWorldPosition() + new Vector3(cellSize * i, 0), 1);
        }
        for(int i = 0; i < h; i++)
        {
            temph += grid.GetY(movableObject.transform.position + new Vector3(0, cellSize * i)) * cellSize + transform.position.y + (cellSize * 0.5f);
            grid.SetValue(GetMaouseWorldPosition() + new Vector3(0, cellSize * i), 1);
        }
        movableObject.transform.position = new Vector2(tempw / w, temph / h);
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            grid.SetValue(GetMaouseWorldPosition(), 56);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetValue(GetMaouseWorldPosition()));
        }
    }
    private Vector3 GetMaouseWorldPosition()
    {
        Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vec.z = 0f;
        return vec;
    }
    public int GetValue(Vector3 worldPosition)
    {
        return grid.GetValue(worldPosition);
    }
    public int GetX(Vector3 worldPosition)
    {
        return grid.GetX(worldPosition);
    }
    public int GetY(Vector3 worldPosition)
    {
        return grid.GetY(worldPosition);
    }
}
