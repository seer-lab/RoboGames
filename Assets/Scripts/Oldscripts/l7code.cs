using UnityEngine;
using System.Collections;

public class l7code : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.GetComponent<TextMesh> ().text = "         compare(part1,part2,similar){" +
			"\n" +
			"\n " +
			"\n" + 
			"\n\t       dif1,dif2 = 0;" + 
			"\n\tdif1 +=                   (part1.power-part2.power);" +
				"\n\tdif1 +=                   (part1.effic-part2.effic);" +
				"\n\tdif1 +=                   (part1.cond-part2.cond);" +
				"\n\tdif2 +=                   (part1.power-similar.power);" +
				"\n\tdif2 +=                   (part1.effic-similar.effic);" +
				"\n\tdif2 +=                   (part1.cond-similar.cond);" +
			"\n\t     (color2.           %6 == 0){                           (part1,part2,similar);}" +
			"\n\t              (dif1 > dif2){               closer;}" +
			"\n\t        {                color2};\n}";
		this.GetComponent<TextMesh> ().color = Color.black;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
