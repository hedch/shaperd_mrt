using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class proj_exp1a_revise : MonoBehaviour {


    //protected SerialPort port = new SerialPort("COM3", 200, Parity.None, 8, StopBits.One);//port connect TXD (white) and GND (black)
    protected string trigger_code = "";

    public string subject = "syw";
    protected string task = "eeg_1";
    public int group_number = 1;
    protected const int set_size = 3;

    protected const int block_size = 120;
    protected const int block_num = 2;

    protected const int c_dly_num = 2;
    protected const int obj_num = 4;
    protected const int ang_num = 5;
    protected const int rep_num = 3;
    // there are also 2 anstwers
    public int wrong = 0;
    protected float s1 = 1f;

    public float _timer = 0f;
    protected float time_begin = 0f;
    protected float[] cue_delay = new float[block_size];
    protected Vector3 tgt_rotation = new Vector3(0, 0, 0);
    protected GameObject start;
    protected GameObject stimuli;
    protected GameObject target;
    protected GameObject match;
    public GameObject reference;
    protected GameObject[] distraction = new GameObject[set_size-1];
    protected GameObject end;
    protected int[][] _tag = new int[block_num*block_size][];
    public float[][] pos_list = new float[set_size][];
    public int index = 0;
    protected float[] rt = new float[block_size+1];
    protected string resp = "";
    protected float[] cor_ans = new float[block_size];
    protected float[] angle = new float[block_size];
    protected bool receiver = true;
    protected bool progress = true;

    protected float loc_z = 45f;
    protected float pos_r = 3f;
    protected float pos_dis = 2f;

    protected float ini_ang = 20f;
    protected float intv_ang = 25f;

    protected GameObject mask;
    protected GameObject fix_cross;
    Vector3 miss_pos = new Vector3(0, 0, -20);
    protected float this_delay = 0;

    // Use this for initialization
    void Start () {
       ini_present_seq();
       start = (GameObject)Resources.Load("start");
        mask = GameObject.Find("mask");
        fix_cross = GameObject.Find("cross");
        match = Instantiate(start, new Vector3(-6.0f, 0.0f, 20.0f), Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f))) as GameObject;
        fix_cross.transform.position = miss_pos;
        mask.transform.position = miss_pos;

    }
	
	// Update is called once per frame
	void Update () {

        _timer += Time.deltaTime;

       

       if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && receiver)
            {
            receiver = false;
            //port.Close();
                rt[index] = _timer - time_begin;

                foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        resp += keyCode.ToString();
                        if (index > 0)
                        {
                            if (int.Parse(resp.Substring(resp.Length - 1, 1)) != cor_ans[index - 1])
                            {
                        
                                wrong++;

                            }
                        }
                }
                }

                Destroy(match);
                mask.transform.position = miss_pos;

            if (index <= block_size-1)
            {
                fix_cross.transform.position = new Vector3(0, 0, loc_z);
                   Invoke("present", 1.5f);
            }else{
                    finish();
            }

            


        }

      

        if (Input.GetKeyDown(KeyCode.UpArrow)  && progress) {

            Time.timeScale = 0;
            progress = false;
            GameObject _plane = GameObject.Find("Plane");
            _plane.GetComponent<Renderer>().material.color = new Color(0, 0, 0, 1);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && (!progress))
        {

            Time.timeScale = 1;
            progress = true;
            GameObject _plane = GameObject.Find("Plane");
            _plane.GetComponent<Renderer>().material.color = new Color(0.627f, 0.627f, 0.627f, 1);

        }

    }

    void present() {
        create_random_position();
        StartCoroutine(pres_target());
        StartCoroutine(pres_cue());
        StartCoroutine(remove_cue());
        StartCoroutine(pres_match());
    }

    IEnumerator pres_target()
    {
        create_target();
       
        create_distraction();
        
        yield return new WaitForSeconds(s1);

        Destroy(target);
        for (int i = 0; i < set_size - 1; i++) {

            Destroy(distraction[i]);
        }

       

        cor_ans[index] = (float)_tag[index+group_number*block_size][3];
        angle[index] = (float)_tag[index + group_number * block_size][2] * intv_ang + ini_ang;
       
    
    }

    IEnumerator pres_cue()
    {
        switch (_tag[index + group_number * block_size][0]) {

            case 0:
                this_delay = 0f;
                trigger_code = "~~";
                break;
            case 1:
                this_delay = 2.0f;
                trigger_code = " ";
                break;
            case 2:
                this_delay = 0.5f;
                trigger_code = " ";
                break;


        }

        cue_delay[index] = this_delay;

        yield return new WaitForSeconds(s1+this_delay);
        mask.transform.position = new Vector3(pos_list[0][0], pos_list[0][1], loc_z);
        //port.Open();
        //port.Write(trigger_code);


    }
    IEnumerator remove_cue(){
        yield return new WaitForSeconds(s1 + this_delay + 1);
        mask.transform.position = miss_pos;
    }

    IEnumerator pres_match(){
        yield return new WaitForSeconds(s1 + 3f);
        create_match();
        index++;
        receiver = true;
        time_begin = _timer;
    }

    void ini_present_seq()
    {

        for (int i = 0; i < block_num*block_size; i++)
        {
            // 2 means the right ans can be either yes or no
            _tag[i] = new int[] { i / (rep_num *2 * ang_num * obj_num) % c_dly_num, i / (rep_num * 2 * ang_num) % obj_num, i / (rep_num * 2) % ang_num, i / rep_num % 2, i % rep_num };
        }

        Random.InitState(12);
        for (int i = block_num * block_size-1; i > 0; i--)
        {
            int p = Random.Range(0, i);
            int[] temp = _tag[p];
            _tag[p] = _tag[i];
            _tag[i] = temp;
        }


    }

    void create_random_position() {

        for (int i = 0; i < set_size; i++) {

            float theta = Random.Range(0f, 360.0f)/360;

            pos_list[i] = new float[] { pos_r * (float)System.Math.Cos(theta * 2 * System.Math.PI), pos_r * (float)System.Math.Sin(theta * 2 * System.Math.PI) };

            for (int j = 0; j < i; j++) {
                
                Vector2 a = new Vector2(pos_list[i][0],pos_list[i][1]);
                Vector2 b = new Vector2(pos_list[j][0], pos_list[j][1]);
                if (Vector2.Distance(a, b) < pos_dis) {
                    i--;
                }

            }

        }

       
    }

    void create_target() {

        switch (_tag[index + group_number * block_size][1])
        {
            case 0:
                stimuli = (GameObject)Resources.Load("A");
                break;
            case 1:
                stimuli = (GameObject)Resources.Load("B");
                break;
            case 2:
                stimuli = (GameObject)Resources.Load("C");
                break;
            case 3:
                stimuli = (GameObject)Resources.Load("D");
                break;
            case 4:
                stimuli = (GameObject)Resources.Load("E");
                break;
            case 5:
                stimuli = (GameObject)Resources.Load("F");
                break;
            case 6:
                stimuli = (GameObject)Resources.Load("G");
                break;
        }

      
        reference.transform.eulerAngles = new Vector3(0, 0, 0);

       
        int y = (int)Random.Range(0f, 4f);
        float z = Random.Range(0f, 360f);
        tgt_rotation = new Vector3(0, y * 180, z);
        reference.transform.Rotate(tgt_rotation, Space.World);
        target = Instantiate(stimuli, new Vector3(pos_list[0][0], pos_list[0][1], loc_z), reference.transform.localRotation) as GameObject;
      
    }

    void create_distraction()
    {

        for (int i = 0; i < set_size - 1; i++)
        {

            int j = (int)Random.Range(0.0f, 6.4f);
            int k = (int)Random.Range(0.0f, 6.4f);
            reference.transform.eulerAngles = new Vector3(0, 0, 0);
            reference.transform.Rotate(new Vector3(0, (i % 2) * 180f, j * (40f)), Space.World);
            switch (k)
            {
                case 0:
                    stimuli = (GameObject)Resources.Load("A");
                    break;
                case 1:
                    stimuli = (GameObject)Resources.Load("B");
                    break;
                case 2:
                    stimuli = (GameObject)Resources.Load("C");
                    break;
                case 3:
                    stimuli = (GameObject)Resources.Load("D");
                    break;
                case 4:
                    stimuli = (GameObject)Resources.Load("E");
                    break;
                case 5:
                    stimuli = (GameObject)Resources.Load("F");
                    break;
                case 6:
                    stimuli = (GameObject)Resources.Load("G");
                    break;
            }



            Vector3 pos = new Vector3(pos_list[i + 1][0], pos_list[i + 1][1], loc_z);
            distraction[i] = Instantiate(stimuli, pos, reference.transform.localRotation) as GameObject;

        }
    }


    void create_match() {
        fix_cross.transform.position = miss_pos;
        mask.transform.position = miss_pos;
        switch (_tag[index + group_number * block_size][1])
        {
            case 0:
                stimuli = (GameObject)Resources.Load("A");
                break;
            case 1:
                stimuli = (GameObject)Resources.Load("B");
                break;
            case 2:
                stimuli = (GameObject)Resources.Load("C");
                break;
            case 3:
                stimuli = (GameObject)Resources.Load("D");
                break;
            case 4:
                stimuli = (GameObject)Resources.Load("E");
                break;
            case 5:
                stimuli = (GameObject)Resources.Load("F");
                break;
            case 6:
                stimuli = (GameObject)Resources.Load("G");
                break;
        }

        Vector3 mt_rotation = new Vector3(0, 0, 0);
        switch (_tag[index + group_number * block_size][3])
        {
            case 0:
                mt_rotation = new Vector3(0, tgt_rotation.y, ini_ang + tgt_rotation.z + _tag[index + group_number * block_size][2] * intv_ang);
                break;
            case 1:
                mt_rotation = new Vector3(0, tgt_rotation.y + 180, ini_ang - tgt_rotation.z + _tag[index + group_number * block_size][2] * intv_ang);
                break;
        }

        reference.transform.eulerAngles = new Vector3(0, 0, 0);
        reference.transform.Rotate(mt_rotation, Space.World);
        match = Instantiate(stimuli, new Vector3(0, 0, loc_z), reference.transform.localRotation) as GameObject;
    }

    void finish()
    {

        end = (GameObject)Resources.Load("finish");

        match = Instantiate(end, new Vector3(-2.0f, 0.0f, 15.0f), Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f))) as GameObject;


        string _rt = "reaction time: " + floatListToString(remove_first(rt)) + "\r\n";
        string _cor_ans = "correct answer: " + floatListToString(cor_ans) + "\r\n";
        string _resp = "response: " + resp.Substring(6) + "\r\n";
        string _angle = "angle: " + floatListToString(angle) + "\r\n";
        string _cue_delay = "cue delay: " + floatListToString(cue_delay) + "\r\n";


        string result = _rt + _cor_ans + _resp + _angle + _cue_delay;
        string file_name = subject + "#result#" + task + "#set size#" + set_size.ToString() + "#group#" + group_number.ToString();

        data_file.Create_file(Application.dataPath, file_name, result);
    }

    string floatListToString(float[] list)
    {
        string str = "";
        foreach (float n in list)
            str += n + "--";

        return str;
    }


    float[] remove_first(float[] input)
    {
        float[] output = new float[input.Length - 1];
        for (int i = 0; i < input.Length - 1; i++)
        {
            output[i] = input[i + 1];

        }

        return output;

    }

    List<T> _sort<T>(List<T> list) {

        var random = new System.Random();
        var newlist = new List<T>();
        foreach (var item in list) {

            newlist.Insert(random.Next(newlist.Count), item);
        }

        return newlist;
    }


}
