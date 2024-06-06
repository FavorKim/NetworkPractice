using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MySql;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections.Generic;
using Mirror;

public class LoginPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField Input_ID;
    [SerializeField] TMP_InputField Input_PW;
    [SerializeField] Button Button_Login;
    [SerializeField] TMP_Text TxT_Result;
    [SerializeField] GameObject _SignIn;
    [SerializeField] GameObject GameObject_MainMenu;

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
                WriteResultTxt("Connected");
                return true;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.ToString());
            WriteResultTxt("Connecting Failed");
            return false;
        }
    }

    private void LogIn(string Id, string Pw)
    {
        if (!ConnectDB())
        {
            WriteResultTxt("Login Failed");
            return;
        }

        Dictionary<string, string> account = GetUserInfo();
        if (!CheckLogin(account, Id, Pw)) return;
        OnLoginSuccess();
    }

    public bool CheckLogin(Dictionary<string, string> account, string id, string pw)
    {
        foreach (string ids in account.Keys)
        {
            if (ids.Equals(id))
            {
                if (pw.Equals(account[ids]))
                {
                    return true;
                }
                else
                    WriteResultTxt("Wrong Password");
            }
        }
        WriteResultTxt("ID Doesn't Exist");
        return false;
    }

    void CreateAccount(string Id, string Pw)
    {
        Dictionary<string, string> account = GetUserInfo();
        if (account == null) return;
        if (account.ContainsKey(Id))
        {
            WriteResultTxt("ID Already being used");
            return;
        }

        if(CreateNewAccount(Id, Pw))
        {
            WriteResultTxt("SignIn Success");
        }
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
            Debug.Log("Loading user info Success");
            return account;
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
            Debug.Log("Loading user info Failed");
            return null;
        }
    }

    public static bool CreateNewAccount(string id, string pw)
    {
        try
        {
            _dbConnection.Open();
            MySqlCommand cmd = new();
            cmd.Connection = _dbConnection;
            cmd.CommandText = $"INSERT INTO user_info (U_ID,U_Password) VALUES('{id}','{pw}');";
            cmd.ExecuteNonQuery();
            _dbConnection.Close();
            Debug.Log("Sign in Success");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e);
            Debug.Log("Sign in Failed");
            return false;
        }
    }

    void WriteResultTxt(string txt)
    {
        TxT_Result.text = txt;
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
        if(string.IsNullOrWhiteSpace(Input_ID.text) || string.IsNullOrWhiteSpace(Input_PW.text))
        {
            WriteResultTxt("Fill in the ID / Password field please");
            return;
        }
        CreateAccount(Input_ID.text, Input_PW.text);
    }

    public void OnClick_SignIn()
    {
        _SignIn.SetActive(true);
    }

    void OnLoginSuccess()
    {
        GameObject_MainMenu.SetActive(true);
        transform.root.gameObject.SetActive(false);
        WriteResultTxt("Login Success");
        var man = NetworkManager.singleton as RoomManager;
        man.localPlayerName = Input_ID.text;
    }
    
}
