using Photon.Pun;
using UnityEngine;

public class PhotonNetworkInitializer : MonoBehaviourPunCallbacks
{
    private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    
        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Photon Master Server");
        }
}
