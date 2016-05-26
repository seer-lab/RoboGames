using UnityEngine;
using System.Collections;

public class l7codeC : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.GetComponent<TextMesh> ().text = "" +
			"\n\t//this function compares part1 to part2 and similar and returns " +
			"\n\t//whichever of part2 and similar is most like part1 in terms of " +
			"\n\t//power level, condition, and efficiency" + 
			"\n\t" + 
			"\n\t                                                                                      //power component" +
			"\n\t                                                                                      //effic component" +
			"\n\t                                                                                      //cond component" +
			"\n\t                                                                                      //power component" +
			"\n\t                                                                                      //effic component" +
			"\n\t                                                                                      //cond component";
		this.GetComponent<TextMesh> ().color = Color.grey;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
