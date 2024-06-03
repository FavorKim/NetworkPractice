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

    private static MySqlConnection _dbConnection;

    [SerializeField] string _server;
    [SerializeField] string _db;
    [SerializeField] string _id;
    [SerializeField] string _pw;




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
        if(!CheckLogin(account, Id, Pw)) return;

        TxT_Result.text = $"Account : {Id} - {Pw} ";



    }

    public bool CheckLogin(Dictionary<string,string> account, string id, string pw)
    {
        foreach(string ids in account.Keys)
        {
            if(ids.Equals(id))
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
        Dictionary<string, string> account = GetUserInfo();
        foreach(string id in account.Keys)
        {
            if(id.Equals(Id))
            {
                Debug.Log("아이디 중복");
                return;
            }
        }
    }

    

    public static Dictionary<string,string> GetUserInfo()
    {
        try
        {
            _dbConnection.Open();
            MySqlCommand sqlCmd = new MySqlCommand();
            sqlCmd.Connection = _dbConnection;
            sqlCmd.CommandText = "SELECT U_ID FROM user_info";

            MySqlDataAdapter adapter = new MySqlDataAdapter(sqlCmd);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "user_info");

            Dictionary<string, string> account = new();
            foreach (DataRow row in dataSet.Tables)
            {
                account.Add(row["U_ID"].ToString(), row["U_Password"].ToString());
            }
            _dbConnection.Close();
            return account;
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
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
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e);
            throw;
        }
    }
    
    
    
    public void OnValueChange_Input(string value)
    {
        Button_Login.interactable = string.IsNullOrWhiteSpace(value) ? false : true;
    }

    public void OnEndEdit_Login(string value)
    {
        LogIn(Input_ID.text, Input_PW.text);
    }

    public void OnClick_LoginBtn()
    {
        LogIn(Input_ID.text, Input_PW.text);
    }

    public void OnEndEdit_CreateAccount(string value)
    {
        CreateAccount(Input_ID.text, Input_PW.text);
    }

    public void OnClick_CreateAccount()
    {
        CreateAccount(Input_ID.text, Input_PW.text);
    }
}
