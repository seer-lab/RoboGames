using UnityEngine;
using System.Collections;

public class L3Output : MonoBehaviour {

	string output = "";
/*	bool initmi = false;
	bool preminmi = false;
	bool premaxmi = false;
	bool postmaxmi = false;
	bool endmi = false;
	bool initma = false;
	bool preminma = false;
	bool premaxma = false;
	bool postmaxma = false;
	bool endma = false;
	bool iprint = false;

	public GameObject Tinitmi;
	public GameObject Tpreminmi;
	public GameObject Tpremaxmi;
	public GameObject Tpostmaxmi;
	public GameObject Tendmi;
	public GameObject Tinitma;
	public GameObject Tpreminma;
	public GameObject Tpremaxma;
	public GameObject Tpostmaxma;
	public GameObject Tendma;
	public GameObject Tiprinter;*/

	bool[] pbool = {false,false,false,false,false,false,false,false};
	bool[] ibool = {false,false,false,false,false};
	bool[] jbool = {false,false,false,false};
	public GameObject[] ptxt = new GameObject[8];
	public GameObject[] itxt = new GameObject[5];
	public GameObject[] jtxt = new GameObject[4];

	// Use this for initialization
	void Start () {
		/*TextMesh t;
		t = Tinitmi.GetComponent<TextMesh>();
		t.text = "";
		t = Tpreminmi.GetComponent<TextMesh>();
		t.text = "";
		t = Tpremaxmi.GetComponent<TextMesh>();
		t.text = "";
		t = Tpostmaxmi.GetComponent<TextMesh>();
		t.text = "";
		t = Tendmi.GetComponent<TextMesh>();
		t.text = "";
		t = Tinitma.GetComponent<TextMesh>();
		t.text = "";
		t = Tpreminma.GetComponent<TextMesh>();
		t.text = "";
		t = Tpremaxma.GetComponent<TextMesh>();
		t.text = ")";
		t = Tpostmaxma.GetComponent<TextMesh>();
		t.text = "";
		t = Tendma.GetComponent<TextMesh>();
		t.text = "";
		t = Tiprinter.GetComponent<TextMesh>();
		t.text = "";*/
	}
	
	// Update is called once per frame
	void Update () {

		for (int i=0; i<8; i++) {
			pbool[i] = ptxt[i].GetComponent<TextMesh>().text == "cout<<priorities;";
		}
		for (int i=0; i<5; i++) {
			//ibool[i] = itxt[i].GetComponent<TextMesh>().text == "cout<<priorities[i];";
			ibool[i] = itxt[i].GetComponent<TextMesh>().text == "cout<<i;";
		}
		for (int i=0; i<4; i++) {
			//jbool[i] = jtxt[i].GetComponent<TextMesh>().text == "cout<<priorities[j];";
			jbool[i] = jtxt[i].GetComponent<TextMesh>().text == "cout<<j;";
		}
		/*t = Tinitmi.GetComponent<TextMesh>();
		initmi = t.text == "Console.WriteLine(min);";
		t = Tpreminmi.GetComponent<TextMesh>();
		preminmi = t.text == "Console.WriteLine(min);";
		t = Tpremaxmi.GetComponent<TextMesh>();
		premaxmi = t.text == "Console.WriteLine(min);";
		t = Tpostmaxmi.GetComponent<TextMesh>();
		postmaxmi = t.text == "Console.WriteLine(min);";
		t = Tendmi.GetComponent<TextMesh>();
		endmi = t.text == "Console.WriteLine(min);";
		t = Tinitma.GetComponent<TextMesh>();
		initma = t.text == "Console.WriteLine(max);";
		t = Tpreminma.GetComponent<TextMesh>();
		preminma = t.text == "Console.WriteLine(max);";
		t = Tpremaxma.GetComponent<TextMesh>();
		premaxma = t.text == "Console.WriteLine(max);";
		t = Tpostmaxma.GetComponent<TextMesh>();
		postmaxma = t.text == "Console.WriteLine(max);";
		t = Tendma.GetComponent<TextMesh>();
		endma = t.text == "Console.WriteLine(max);";
		t = Tiprinter.GetComponent<TextMesh>();
		iprint = t.text == "Console.WriteLine(i);";*/
		setText ();
	}

	string printArray(int[] ar){
		output = "[";
		for (int i = 0;i<ar.Length;i++){
			output += System.Convert.ToString(ar[i]);
			if (i<ar.Length-1){
				output += ",";
			}
		}
		output += "]";
		return output;
	} 

	void setText() {
		output = "";
		int[] values = {1,3,0,4,2};
		int temp;
		if (pbool [0]) {
			output += printArray(values) + "\n";
		}
		for (int i = 1; i<5; i++) {
			if (ibool[0]){
				//output += "priorities[i] = " + System.Convert.ToString(values[i]) + "\n";
				output += "i = " + System.Convert.ToString(i) + "\n";
			}
			if (pbool[1]){
				output += printArray(values) + "\n";
			}
			for(int j = 1;j<5;j++){
				if (ibool[1]){
					//output += "priorities[i] = " + System.Convert.ToString(values[i]) + "\n";
					output += "i = " + System.Convert.ToString(i) + "\n";
				}
				if (pbool[2]){
					output += printArray(values) + "\n";
				}
				if (jbool[0]){
					//output += "priorities[j] = " + System.Convert.ToString(values[j]) + "\n";
					output += "j = " + System.Convert.ToString(j) + "\n";
				}

				if (values[i]>values[j]){
					if (ibool[2]){
						//output += "priorities[i] = " + System.Convert.ToString(values[i]) + "\n";
						output += "i = " + System.Convert.ToString(i) + "\n";
					}
					if (pbool[3]){
						output += printArray(values) + "\n";
					}
					if (jbool[1]){
						//output += "priorities[j] = " + System.Convert.ToString(values[j]) + "\n";
						output += "j = " + System.Convert.ToString(j) + "\n";
					}
					temp = values[i];
					values[i] = values[j];
					values[j] = temp;
					if (ibool[3]){
						//output += "priorities[i] = " + System.Convert.ToString(values[i]) + "\n";
						output += "i = " + System.Convert.ToString(i) + "\n";
					}
					if (pbool[4]){
						output += printArray(values) + "\n";
					}
					if (jbool[2]){
						//output += "priorities[j] = " + System.Convert.ToString(values[j]) + "\n";
						output += "j = " + System.Convert.ToString(j) + "\n";
					}
				}

				if (ibool[4]){
					//output += "priorities[i] = " + System.Convert.ToString(values[i]) + "\n";
					output += "i = " + System.Convert.ToString(i) + "\n";
				}
				if (pbool[5]){
					output += printArray(values) + "\n";
				}
				if (jbool[3]){
					//output += "priorities[j] = " + System.Convert.ToString(values[j]) + "\n";
					output += "j = " + System.Convert.ToString(j) + "\n";
				}
			}
			if (pbool[6]){
				output += printArray(values) + "\n";
			}
		}
		if (pbool[7]){
			output += printArray(values) + "\n";
		}
	/*	if (initmi) {
			output += "Min = 0\n";
		}
		if (initma){
			output += "Max = 0\n";
		}
		int min = 0;
		int max = 0;
		int[] values = {0,4,1,6,3,2,5};
		for (int i =0; i<7;i++){
			if (iprint){
				output += "i = " + System.Convert.ToString(i) + "\n"; 
			}
			if (preminmi){
				output += "Min = " + System.Convert.ToString(min) + "\n";
			}
			if (preminma){
				output += "Max = " + System.Convert.ToString(max) + "\n";
			}
			if (i==4){
				min = 6;
			}
			else {
				min = System.Math.Min(min, values[i]);
			}
			if (premaxmi){
				output += "Min = " + System.Convert.ToString(min) + "\n";
			}
			if (premaxma){
				output += "Max = " + System.Convert.ToString(max) + "\n";
			}
			if (i==4){
				max = 0;
			}
			else {
				max = System.Math.Max(max, values[i]);
			}
			if (postmaxmi){
				output += "Min = " + System.Convert.ToString(min) + "\n";
			}
			if (postmaxma){
				output += "Max = " + System.Convert.ToString(max) + "\n";
			}
		}
		if (endmi) {
			output += "Min = " + System.Convert.ToString(min) + "\n";
		}
		if (endma) {
			output += "Max = " + System.Convert.ToString(max) + "\n";
		}*/
		TextMesh Tm = GetComponent<TextMesh>();
		Tm.text = output;
		Tm.color = Color.white;
	}
}
