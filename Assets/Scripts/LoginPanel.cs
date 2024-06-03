using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MySql;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections.Generic;

public class LoginPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField Input_ID;
    [SerializeField] TMP_InputField Input_PW;
    [SerializeField] Button Button_Login;
    [SerializeField] TMP_Text TxT_Result;
    [SerializeField] GameObject _SignIn;

    private static MySqlConnection _dbConnection;

    [SerializeField] string _server;
    [SerializeField] string _db;
    [SerializeField] string _id;
    [SerializeField] string _pw;


    private void Awake()
    {
        if (_SignIn != null)
            _SignIn.SetActive(false);
        ConnectDB();
    }

    bool ConnectDB()
    {
        string connectStr = $"Server={_server};Database={_db};Uid={_id};Pwd={_pw};";

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectStr))
            {
                _dbConnection = conn;
                //_dbConnection.Open();
                Debug.Log("연결 성공");
                return true;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.ToString());
            Debug.Log("연결 실패");
            return false;
        }
    }

    private void LogIn(string Id, string Pw)
    {
        if (!ConnectDB())
        {
            TxT_Result.text = "Can't Connect DB";
            return;
        }

        Dictionary<string, string> account = GetUserInfo();
        if (!CheckLogin(account, Id, Pw)) return;

        TxT_Result.text = $"Account : {Id} - {Pw} ";



    }

    public bool CheckLogin(Dictionary<string, string> account, string id, string pw)
    {
        foreach (string ids in account.Keys)
        {
            if (ids.Equals(id))
            {
                if (pw.Equals(account[ids]))
                {
                    Debug.Log("로그인 성공");
                    return true;
                }
                else
                    Debug.Log("비밀번호가 다릅니다");
            }
        }
        Debug.Log("로그인 실패");
        return false;
    }

    void CreateAccount(string Id, string Pw)
    {
        Debug.Log("id : "+Id +" pw : " + Pw);

        Dictionary<string, string> account = GetUserInfo();
        if (account == null) return;
        if (account.ContainsKey(Id))
        {
            Debug.Log("아이디 중복");
            return;
        }

        CreateNewAccount(Id, Pw);
    }



    public static Dictionary<string, string> GetUserInfo()
    {
        try
        {
            _dbConnection.Open();
            MySqlCommand sqlCmd = new MySqlCommand();
            sqlCmd.Connection = _dbConnection;
            sqlCmd.CommandText = "SELECT * FROM user_info ";

            MySqlDataAdapter adapter = new MySqlDataAdapter(sqlCmd);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "user_info");

            Dictionary<string, string> account = new();
            foreach (DataRow row in dataSet.Tables["user_info"].Rows)
            {
                account.Add(row["U_ID"].ToString(), row["U_Password"].ToString());
            }
            _dbConnection.Close();
            Debug.Log("유저 정보 읽어오기 성공");
            return account;
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
            Debug.Log("유저 정보 읽어오기 실패");
            return null;
        }
    }

    public static void CreateNewAccount(string id, string pw)
    {
        try
        {
            _dbConnection.Open();
            MySqlCommand cmd = new();
            cmd.Connection = _dbConnection;
            cmd.CommandText = $"INSERT INTO user_info (U_ID,U_Password) VALUES('{id}','{pw}');";
            cmd.ExecuteNonQuery();
            _dbConnection.Close();
            Debug.Log($"{id},{pw}");
            Debug.Log("회원가입 성공");
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e);
            Debug.Log("회원가입 실패");
            throw;
        }
    }



    public void OnValueChange_Input(string value)
    {
        Button_Login.interactable = string.IsNullOrWhiteSpace(value) ? false : true;
    }

    public void OnClick_LoginBtn()
    {
        LogIn(Input_ID.text, Input_PW.text);
    }

    public void OnClick_CreateAccount()
    {
        Debug.Log(Input_ID);
        Debug.Log(Input_PW);
        CreateAccount(Input_ID.text, Input_PW.text);
    }

    public void OnClick_SignIn()
    {
        _SignIn.SetActive(true);
    }
}
