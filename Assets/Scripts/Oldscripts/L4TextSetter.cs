using UnityEngine;
using System.Collections;

public class L4TextSetter : MonoBehaviour {

	string main = "" +
		"\n" +
		"\n" +
		"\n" +
		"\n" +
			"\n         <stdio.h>" +
			"\n         <colourmanager.h>" +
			"\n        TOTAL_COLOURS   12" +
		"\n       colour {" +
			"\n         name[28];" +
			"\n        red;" +
			"\n        green;" +
			"\n        blue;};" +
		"\n             tables {" +
			"\n           colour *table;" +
			"\n        tablesize;" +
		"\n} coltable[TOTAL_COLOURS];" +
		"\n\n     colourDatabase() {" +
			"\n         colours { WHITE, GREY, BLACK, RED, BROWN, " +
			"\n    ORANGE, YELLOW, GREEN, CYAN, BLUE, " +
			"\n    MAGENTA, NAMED } colour;" +
			"\n         *colourname[] = { \"white\", \"grey\", \"black\", \"red\", " +
			"\n    \"brown\", \"orange\", \"yellow\", \"green\", \"cyan\", \"blue\"," +
			"\n    \"magenta\", \"named colours\" };" +
			"\n        i = 0;\n        red = 0, green = 0, blue = 0;\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n" +
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
*/		"\n\n       (    colour = 0; colour < TOTAL_COLOURS; colour++) {" +
	//	"\n  printf(\"" +
	//	"\n ==> %s <==" +
	//	"\n\", colourname[colour]);" +
			"\n           (    i = 0; i < coltable[colour].tablesize; i++) {" +
			"\n                red   = coltable[colour].table[i].red;" +
			"\n                green = coltable[colour].table[i].green;" +
			"\n                blue  = coltable[colour].table[i].blue;" +
			"\n                   name = coltable[colour].table[i].name;" + 
			"\n                     (red, green, blue, name);" +
	/*	"\n   printf(\" %03d %03d %03d - #%02x%02x%02x - %s" +
		"\n\", " +
		"\n     red, green, blue, red, green, blue, coltab[colour].table[i].name);" +
		"\n  }" +
		"\n }" +
		"\n" +
		"\n return 0;" +
		"\n}" +
		"\n" +*/
			"\n                }\n        }\n}";

	// Use this for initialization
	void Start () {
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = main;	
		Tm.color = Color.white;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
