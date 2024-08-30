using CSA;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{

    bool trig, open;//trig-проверка входа выхода в триггер(игрок должен быть с тегом Player) open-закрыть и открыть дверь
    public float smooth = 2.0f;//скорость вращения
    public float DoorOpenAngle = 90.0f;//угол вращения 
    private Vector3 defaulRot;
    private Vector3 openRot;
    public Text txt;//text 
    // Start is called before the first frame update
    void Start()
    {
        defaulRot = transform.eulerAngles;
        openRot = new Vector3(defaulRot.x, defaulRot.y + DoorOpenAngle, defaulRot.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (open)//открыть
        {
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, openRot, Time.deltaTime * smooth);
        }
        else//закрыть
        {
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, defaulRot, Time.deltaTime * smooth);
        }

        if (Input.GetKeyDown(KeyCode.E) && trig)
        {
            open = !open;
        }

        if (trig)
        {
            if (open)
            {
                txt.text = "Close E";
            }
            else
            {
                txt.text = "Open E";
            }
        }
    }
    private void OnTriggerStay(Collider coll)//вход и выход в\из  триггера 
    {
        //if (!coll.CompareTag(Tags.Player))
        //    return;

        Vector3 dir = transform.position - coll.transform.position;
        if (Vector3.Dot(dir, coll.transform.forward) < 0)
        {
            txt.text = "";
            trig = false;
        }
        else
        {
            if (!open)
            {
                txt.text = "Close E";
            }
            else
            {
                txt.text = "Open E";
            }
            trig = true;
        }
        
    }
    private void OnTriggerExit(Collider coll)//вход и выход в\из  триггера 
    {
        //if (coll.CompareTag(Tags.Player))
        //{
        //    txt.text = " ";
        //    trig = false;
        //}
    }
}
