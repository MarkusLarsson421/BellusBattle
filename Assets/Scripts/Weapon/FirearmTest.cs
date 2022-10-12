using UnityEngine;

public class FirearmTest : MonoBehaviour{
	[SerializeField] [Tooltip("")]
	private GameObject firearms;

	private BallisticFirearm _bfs;

	private void Start(){
		_bfs = firearms.GetComponent<BallisticFirearm>();
	}

	private void Update(){
		if (Input.GetKey(KeyCode.Mouse0)){
			_bfs.Fire();
		}
	}
}
