using UnityEngine;
using System.Collections;

public class L4TextSetterK : MonoBehaviour {

	string main = "" +
		"\n" +
		"\n" +
		"\n" +
		"\n" +
		"\n#include" +
		"\n#include" +
		"\n#define" +
		"\nstruct" +
			"\n    char" +
			"\n    int" +
			"\n    int" +
			"\n    int" +
		"\nstruct " +
			"\n    struct " +
			"\n    int" +
		"\n" +
		"\n\nvoid" +
			"\n    enum" +
		"\n" +
		"\n" +
			"\n    char " +
		"\n" +
		"\n" +
			"\n    int           \n    int\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n" +
/*		"\n\n coltab[WHITE].table = whitetab;" +
		"\n coltab[WHITE].tabsize = sizeof(whitetab) / sizeof(whitetab[0]);" +
		"\n coltab[GREY].table = greytab;" +
		"\n coltab[GREY].tabsize = sizeof(greytab) / sizeof(greytab[0]);" +
		"\n coltab[BLACK].table = blacktab;" +
		"\n coltab[BLACK].tabsize = sizeof(blacktab) / sizeof(blacktab[0]);" +
		"\n coltab[RED].table = redtab;" +
		"\n coltab[RED].tabsize = sizeof(redtab) / sizeof(redtab[0]);" +
		"\n coltab[BROWN].table = browntab;" +
		"\n coltab[BROWN].tabsize = sizeof(browntab) / sizeof(browntab[0]);" +
		"\n coltab[ORANGE].table = orangetab;" +
		"\n coltab[ORANGE].tabsize = sizeof(orangetab) / sizeof(orangetab[0]);" +
		"\n coltab[YELLOW].table = yellowtab; " +
		"\n coltab[YELLOW].tabsize = sizeof(yellowtab) / sizeof(yellowtab[0]);" +
		"\n coltab[GREEN].table = greentab;" +
		"\n coltab[GREEN].tabsize = sizeof(greentab) / sizeof(greentab[0]);" +
		"\n coltab[CYAN].table = cyantab;" +
		"\n coltab[CYAN].tabsize = sizeof(cyantab) / sizeof(cyantab[0]);" +
		"\n coltab[BLUE].table = bluetab;" +
		"\n coltab[BLUE].tabsize = sizeof(bluetab) / sizeof(bluetab[0]);" +
		"\n coltab[MAGENTA].table = magentatab;" +
		"\n coltab[MAGENTA].tabsize = sizeof(magentatab) / sizeof(magentatab[0]);" +
		"\n coltab[NAMED].table = namedtab;" +
		"\n coltab[NAMED].tabsize = sizeof(namedtab) / sizeof(namedtab[0]);" +
*/		"\n\n    for int" +
	//	"\n  printf(\"" +
	//	"\n ==> %s <==" +
	//	"\n\", colourname[color]);" +
			"\n        for int" +
			"\n            int" +
			"\n            int" +
			"\n            int" +
			"\n            string" + 
			"\n            loadcolor" +
	/*	"\n   printf(\" %03d %03d %03d - #%02x%02x%02x - %s" +
		"\n\", " +
		"\n     red, green, blue, red, green, blue, coltab[color].table[i].name);" +
		"\n  }" +
		"\n }" +
		"\n" +
		"\n return 0;" +
		"\n}" +
		"\n" +*/
		"\n";

	// Use this for initialization
	void Start () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = main;	
		Tm.color = new Color(61f/255f, 189f/255f, 232f/255f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
