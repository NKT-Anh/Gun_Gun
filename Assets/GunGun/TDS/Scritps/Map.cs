using UnityEngine;
public class Map : MonoBehaviour 
{ 
    public Transform playerSpawnPoint; 
    [SerializeField] private Transform[] m_AiSpawnPoint; 
    public Transform RandomAiSP
    { get 
        { 
            if (m_AiSpawnPoint == null || m_AiSpawnPoint.Length <= 0) 
                return null; 
            int randomIdx = Random.Range(0, m_AiSpawnPoint.Length); 
            return m_AiSpawnPoint[randomIdx]; 
        } 
    } 
}