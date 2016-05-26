using UnityEngine;
using System.Collections;

public class l7codeD : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.GetComponent<TextMesh> ().text = "part compare(part1,part2,similar){" +
			"\n//this function compares part1 to part2 and similar and returns " +
			"\n//whichever of part2 and similar is most like part1 in terms of " +
			"\n//power level, condition, and efficiency" + 
			"\n\tint dif1,dif2 = 0;" + 
			"\n\tdif1 += Math.abs(part1.power-part2.power); //power component" +
			"\n\tdif1 += Math.abs(part1.effic-part2.effic); //effic component" +
			"\n\tdif1 += Math.abs(part1.cond-part2.cond); //cond component" +
			"\n\tdif2 += Math.abs(part1.power-similar.power); //power component" +
			"\n\tdif2 += Math.abs(part1.effic-similar.effic); //effic component" +
			"\n\tdif2 += Math.abs(part1.cond-similar.cond); //cond component" +
			"\n\tif (color2.index %6 == 0){return compare(part1,part2,similar);}" +
			"\n\telse if (dif1 > dif2){return closer;}" +
			"\n\telse{return color2};";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
