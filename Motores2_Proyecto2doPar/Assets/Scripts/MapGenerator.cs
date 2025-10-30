using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // cantidad maxima de cuartos a crear
    public int maxRooms = 5;
    // cantidad maxima de hijos a crear por vertice
    public int maxChilds = 2;
    public float roomSize = 10;
    public float hallLength = 10;
    public List<GameObject> roomPrefab;
    public List<GameObject> hallPrefab;
    private List<Vertex> roomList = new List<Vertex>();
    private Dictionary<Vertex, Vector3> vertexPositions = new Dictionary<Vertex, Vector3>();
    private void Start()
    {
        createVertexMap();
        if (maxChilds > 3) { maxChilds = 3; }
    }
    private void createVertexMap()
    {
        // creamos node Root
        roomList.Add(new Vertex(0,"node " + 0));
        for (int i = 0; i < maxRooms; i++)
        {
            // creamos el numero random de cuartos por agregaer
            int RandomRoomsAcount = UnityEngine.Random.Range(1, maxChilds+1);
            List<Vertex> tempList = new List<Vertex>();
            for (int j = roomList.Count; j < roomList.Count + RandomRoomsAcount; j++)
            {
                if (j < maxRooms)
                {
                    tempList.Add(new Vertex(roomList.Count, "node " + j , roomList[i]));
                }
            }
            if (tempList.Count > 0)
            {
                roomList.AddRange(tempList);
                roomList[i].Edges = tempList;
            }
            if (roomList.Count >= maxRooms) { break; }
        }   
        createMap();
    }

   public void createMap()
   {
       vertexPositions.Clear();
       Queue<Vertex> queue = new Queue<Vertex>();
       HashSet<Vector3> usedPositions = new HashSet<Vector3>();
   
       Vertex root = roomList[0];
       vertexPositions[root] = Vector3.zero;
       usedPositions.Add(Vector3.zero);
       queue.Enqueue(root);
   
       while (queue.Count > 0)
       {
           Vertex current = queue.Dequeue();
           Vector3 parentPos = vertexPositions[current];
   
           // Alternamos direcciones cardinales para hijos: +X, -X, +Z, -Z
           Vector3[] directions = new Vector3[]
           {
               new Vector3(roomSize + hallLength, 0, 0),
               new Vector3(-(roomSize + hallLength), 0, 0),
               new Vector3(0, 0, roomSize + hallLength),
               new Vector3(0, 0, -(roomSize + hallLength))
           };
   
           int dirIndex = 0;
           foreach (var child in current.Edges)
           {
               Vector3 candidatePos;
               do
               {
                   candidatePos = parentPos + directions[dirIndex % directions.Length];
                   dirIndex++;
               }
               while (usedPositions.Contains(candidatePos) && dirIndex < directions.Length * 2);
   
               vertexPositions[child] = candidatePos;
               usedPositions.Add(candidatePos);
               queue.Enqueue(child);
           }
       }
   
       foreach (var room in roomList)
       {
            int randomRoom = Random.Range(0,roomPrefab.Count);
           Instantiate(roomPrefab[randomRoom], vertexPositions[room], Quaternion.identity);
       }
   
       foreach (var room in roomList)
       {
           if (room.ParentVertex != null)
           {
               Vector3 start = vertexPositions[room.ParentVertex];
               Vector3 end = vertexPositions[room];
               Vector3 direction = end - start;
               Vector3 midPoint = start + direction / 2f;
               Quaternion rotation = Quaternion.LookRotation(direction);
   
               Instantiate(hallPrefab[0], midPoint, rotation);
           }
       }
   }
}