using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class main_func : MonoBehaviour {

    

	public float _timer = 0f;


	protected float [] pt = new float[201];
	protected float [] et = new float[201];

	protected int index = 0;
	protected string resp="";
	protected float [] cor_ans = new float[201];
	protected float [] angle = new float[201];
	protected int [][] alltag = new int [1600][]; 
	protected int [][] _tag = new int [200][]; 
	protected float [][] view = new float [10][];
    protected string out_view = "";
    protected float ts = 0f;
    protected string seq = "";
    //-------------------parameters setting-----------------------------
    protected string type = "sequence";
	protected string subject = "alex";
	protected int session_num = 7;
    protected string memory_period = "sensory";
    protected string show_loc = "diff";


    //-------------------------------------------------------------------
    protected Vector3 loc_a = new Vector3();
    protected Vector3 loc_b = new Vector3();
    protected float time_pres_a = 0f;
    protected float time_delay = 0f;

    protected GameObject stimuli_a;
	protected GameObject stimuli_b;
	protected GameObject start;
	protected GameObject end;
	protected GameObject on_present_a;
	protected GameObject on_present_b;

	public GameObject reference_a;
	public GameObject reference_b;


	// Use this for initialization
	void Start () {

		for (int i = 0; i < 1600; i++) {

			alltag [i] = new int[]{i/400%4, i/40%10, i/20%2, i/10%2, i%10 };
		
	}

        switch (session_num) {

            case 1:
                stimuli_session_1();
                break;
            case 2:
                stimuli_session_2();
                break;
            case 3:
                stimuli_session_3();
                break;
            case 4:
                stimuli_session_4();
                break;
            case 5:
                stimuli_session_5();
                break;
            case 6:
                stimuli_session_6();
                break;
            case 7:
                stimuli_session_7();
                break;
            case 8:
                stimuli_session_8();
                break;



        }


        if (type == "sequence")
        {

            switch (memory_period)
            {


                case "sensory":
                    time_delay = 6.0f;
                    time_pres_a = 3.0f;

                    break;
                case "short":
                    time_delay = 0.0f;
                    time_pres_a = 0f;

                    break;
                case "long":
                    time_delay = 0.0f;
                    time_pres_a = 0f;

                    break;



            }

            switch (show_loc)
            {


                case "same":
                    loc_a = new Vector3(0f, 0f, 0f);
                    loc_b = new Vector3(0f, 0f, 0f);
                    break;

                case "diff":
                    loc_a = new Vector3(-3.5f, 0.0f, 25.0f);
                    loc_b = new Vector3(3.5f, 0.0f, 25.0f);
                    break;




            }

        }


        for (int i = 199; i > 0; i--)
        {

            UnityEngine.Random.InitState(20);
            int p = UnityEngine.Random.Range(0, i);
            int[] temp = _tag[p];
            _tag[p] = _tag[i];
            _tag[i] = temp;


        }



        for (int i = 0; i < 10; i++) {
		
			if (i == 0 || i == 1) {
				view[i]=new float[]{-38.0f, -68.0f, -88.0f, -18.0f };
			}else if (i == 2 || i == 3){
				view[i]=new float[]{-60.0f, -240.0f, -216.0f, -38.0f };
			}else if (i == 4){
				view[i]=new float[]{-220.0f, 138.0f, 305.0f, 325.0f };
			}else{
				view[i]=new float[]{-55.0f, -215.0f, 130.0f, 290.0f };
			}
		
		}

        for (int i = 0; i < 200; i++) {
            out_view = out_view + _tag[i][0].ToString();
        }



	    start = (GameObject)Resources.Load("start");

		on_present_a = Instantiate (start, new Vector3(-6.0f, 0.0f, 20.0f), Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f))) as GameObject;

	}
	
	// Update is called once per frame
	void Update () {


		_timer += Time.deltaTime;




		if (Input.anyKeyDown) {

			et [index] = _timer;

			Destroy (on_present_a);



			if (index < 200) {


				foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode))) {
					if (Input.GetKeyDown (keyCode)) {
						resp += keyCode.ToString ();
					}
				}


				cor_ans [index] = (float)_tag [index] [2];
				angle [index] = (float)_tag [index] [4] * 20;


				absent ();

                if (type == "simultaneous") {

                    Invoke("present_simultaneous", 1);
                }
                else {

                    Invoke("present_sequence", 1);
                }
                //Invoke ("test", 1);




            } else if (index == 200) {

                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        resp += keyCode.ToString();
                    }
                }

                finish ();
				index++;

			} else {

				Debug.Log ("别摁了");
			}




		}
	}

    void stimuli_session_1() {

        for (int i = 0; i < 200; i++) {

            _tag[i] = alltag[i];

        }


    }

    void stimuli_session_3()
    {
        for (int i = 0; i < 200; i++)
        {

            _tag[i] = alltag[i+400];

        }



    }

    void stimuli_session_5()
    {
        for (int i = 0; i < 200; i++)
        {

            _tag[i] = alltag[4*i + 800];

        }



    }

    void stimuli_session_7()
    {
        for (int i = 0; i < 200; i++)
        {

            _tag[i] = alltag[4 * i + 801];

        }



    }

    void stimuli_session_2()
    {

        for (int i = 0; i < 200; i++)
        {

            _tag[i] = alltag[i+200];

        }


    }

    void stimuli_session_4()
    {
        for (int i = 0; i < 200; i++)
        {

            _tag[i] = alltag[i + 600];

        }



    }

    void stimuli_session_6()
    {
        for (int i = 0; i < 200; i++)
        {

            _tag[i] = alltag[4 * i + 802];

        }



    }

    void stimuli_session_8()
    {
        for (int i = 0; i < 200; i++)
        {

            _tag[i] = alltag[4 * i + 803];

        }



    }


    void absent(){

		Destroy (on_present_a);
		Destroy (on_present_b);

	}


	void present_simultaneous(){





		String pref = "s"+(_tag [index] [1]+1).ToString();

        float y = view[_tag [index] [1]][_tag [index] [0]];
        float x = UnityEngine.Random.Range(0.0f, 180.0f);
		float z = UnityEngine.Random.Range(0.0f, 180.0f);

		float ang_diff = (float)(20 * _tag [index] [4]);





		if (_tag [index] [2] == 0) {

			stimuli_a = (GameObject)Resources.Load(pref+"a");
			stimuli_b = (GameObject)Resources.Load(pref+"a");
			
		}else if(_tag [index] [2] == 1){
			stimuli_a = (GameObject)Resources.Load(pref+"a");
			stimuli_b = (GameObject)Resources.Load(pref+"b");
		}

		if (_tag [index] [0] == 0 || _tag [index] [0] == 2) {


			reference_a.transform.eulerAngles = new Vector3 (x, y, z);
			reference_b.transform.eulerAngles = new Vector3 (x, y, z);
            reference_b.transform.Rotate(new Vector3(ang_diff,0,0), Space.World);
            reference_a.transform.Rotate(new Vector3(0, -19.0f, 0), Space.World);



        }
        else if(_tag [index] [0] == 1 || _tag [index] [0] == 3){

			reference_a.transform.eulerAngles = new Vector3 (x, y, z);
			reference_b.transform.eulerAngles = new Vector3 (x, y, z);
            reference_b.transform.Rotate(new Vector3(0, 0, ang_diff), Space.World);
            reference_a.transform.Rotate(new Vector3(0, -19.0f, 0), Space.World);


        }

		Quaternion rotation_a = reference_a.transform.localRotation;
		Quaternion rotation_b = reference_b.transform.localRotation;

		String info = _tag [index] [0].ToString () + _tag [index] [1].ToString () + _tag [index] [2].ToString () + _tag [index] [3].ToString () + _tag [index] [4].ToString ();
		Debug.Log (info);

		on_present_a = Instantiate (stimuli_a, new Vector3(-3.5f,0.0f,25.0f), rotation_a) as GameObject;
		on_present_b = Instantiate (stimuli_b, new Vector3(3.5f,0.0f,25.0f), rotation_b) as GameObject;


		pt [index] = _timer;

		index++;

	}




    void present_sequence()
    {





        String pref = "s" + (_tag[index][1] + 1).ToString();

        float y = view[_tag[index][1]][_tag[index][0]];
        float x = UnityEngine.Random.Range(0.0f, 180.0f);
        float z = UnityEngine.Random.Range(0.0f, 180.0f);

        float ang_diff = (float)(20 * _tag[index][4]);





        if (_tag[index][2] == 0)
        {

            stimuli_a = (GameObject)Resources.Load(pref + "a");
            stimuli_b = (GameObject)Resources.Load(pref + "a");

        }
        else if (_tag[index][2] == 1)
        {
            stimuli_a = (GameObject)Resources.Load(pref + "a");
            stimuli_b = (GameObject)Resources.Load(pref + "b");
        }

        if (_tag[index][0] == 0 || _tag[index][0] == 2)
        {


            reference_a.transform.eulerAngles = new Vector3(x, y, z);
            reference_b.transform.eulerAngles = new Vector3(x, y, z);
            reference_b.transform.Rotate(new Vector3(ang_diff, 0, 0), Space.World);
            reference_a.transform.Rotate(new Vector3(0, -19.0f, 0), Space.World);



        }
        else if (_tag[index][0] == 1 || _tag[index][0] == 3)
        {

            reference_a.transform.eulerAngles = new Vector3(x, y, z);
            reference_b.transform.eulerAngles = new Vector3(x, y, z);
            reference_b.transform.Rotate(new Vector3(0, 0, ang_diff), Space.World);
            reference_a.transform.Rotate(new Vector3(0, -19.0f, 0), Space.World);


        }

        Quaternion rotation_a = reference_a.transform.localRotation;
        Quaternion rotation_b = reference_b.transform.localRotation;

        String info = _tag[index][0].ToString() + _tag[index][1].ToString() + _tag[index][2].ToString() + _tag[index][3].ToString() + _tag[index][4].ToString();
        //Debug.Log(info);

       

        StartCoroutine(pres_a(stimuli_a, loc_a, rotation_a));


        StartCoroutine(pres_b(stimuli_b, loc_b, rotation_b));



        

    }

    IEnumerator pres_a(GameObject stimuli, Vector3 loc, Quaternion rotation)
    {
        on_present_a = Instantiate(stimuli, loc, rotation) as GameObject;

        yield return new WaitForSeconds(time_pres_a);

        Destroy(on_present_a);
    }

    IEnumerator pres_b(GameObject stimuli, Vector3 loc, Quaternion rotation)
    {
        

        yield return new WaitForSeconds(time_pres_a+time_delay);

        on_present_b = Instantiate(stimuli, loc, rotation) as GameObject;

        pt[index] = _timer;

        index++;
    }

    void finish(){
		Destroy (on_present_a);
		Destroy (on_present_b);

		end = (GameObject)Resources.Load("finish");

		on_present_b = Instantiate (end, new Vector3(-2.0f, 0.0f, 15.0f), Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f))) as GameObject;


		string _pt = "present time: "+floatListToString (pt)+"\r\n";
		string _et = "ending time: "+floatListToString (remove_first (et))+"\r\n";
		string _cor_ans = "correct answer: "+floatListToString (cor_ans)+"\r\n";
		string _resp = "response: "+resp.Substring(6)+"\r\n";
		string _angle = "angle: " + floatListToString (angle)+"\r\n";
        string _view = "view: " + out_view;
		string result = _pt + _et + _cor_ans + _resp + _angle + _view;
		string file_name = type +"&"+memory_period+"&" + show_loc + "&" + subject +"&"+ session_num.ToString();

		data_file.Create_file (Application.dataPath, file_name, result);


	}


	string floatListToString(float[] list){
		string str="";
		foreach(float n in list) 
			str+=n+"--";

		return str;
	}


	float [] remove_first(float [] input){
		float [] output = new float[input.Length-1];
		for (int i = 0; i < input.Length-1; i++) {
			output [i] = input [i + 1];

		}

		return output;

	}




	void test(){

		float y = view[_tag [index] [0]][_tag [index] [1]];

		float x = UnityEngine.Random.Range(0.0f, 180.0f);
		float z = UnityEngine.Random.Range(0.0f, 180.0f);

		//float ang_diff = (float)(20 * _tag [index] [4]);
	

		reference_a.transform.eulerAngles = new Vector3 (x, y, z);
		stimuli_b = (GameObject)Resources.Load("s1a");
		Quaternion rotation_a = reference_a.transform.localRotation;
		on_present_b = Instantiate (stimuli_b, new Vector3(4.0f,0.0f,10.0f), rotation_a) as GameObject;
	}

}
