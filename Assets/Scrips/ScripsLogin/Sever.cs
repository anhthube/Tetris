using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Sever : MonoBehaviour
{
    [SerializeField] GameObject WebcomPanel;
    [SerializeField] GameObject LoginPanel;
    [SerializeField] GameObject RegisterPanel;
    [SerializeField] GameObject SuccessPanel;
    [SerializeField] Text user;

    [SerializeField] InputField UserName;
    [SerializeField] InputField Password;


    [SerializeField] InputField RegisterUserName;
    [SerializeField] InputField RegisterPassword;
    [SerializeField] InputField RegisterConfirmPassword;


    [SerializeField] Text errorMessages;
    [SerializeField] Text RegisterErrorMessages;
    [SerializeField] GameObject progressCircle;

    [SerializeField] Button LoginButton;
    [SerializeField] Button BackLoginButton;
    [SerializeField] Button RegisterButton;
    [SerializeField] Button CallButton;
    private string url = "http://localhost:3000";



    public void onloginButtoncliked()
    {
        LoginButton.interactable = false; // Vô hiệu hóa nút login
        progressCircle.SetActive(true);  // Hiển thị vòng tròn tải
        if (string.IsNullOrEmpty(UserName.text) || string.IsNullOrEmpty(Password.text))
        {
            errorMessages.text = "Tên đăng nhập và mật khẩu không được để trống!";
            LoginButton.interactable = true;
            progressCircle.SetActive(false);
            return; // Không tiếp tục gửi yêu cầu
        }
        StartCoroutine(Login());
    }

    IEnumerator Login()
    {
        // Chuẩn bị form dữ liệu gửi lên server, gui du lieu duoi dang application/x-www-form-urlencoded
        WWWForm form = new WWWForm();
        form.AddField("username", UserName.text);
        form.AddField("password", Password.text);

        using (UnityWebRequest www = UnityWebRequest.Post(url + "/login", form))
        {
            // Gửi yêu cầu tới server
            yield return www.SendWebRequest();

            // Xử lý các lỗi kết nối hoặc HTTP
            if (www.error != null)
            {
                errorMessages.text = "Tên đăng nhập hoặc mật khẩu không đúng "; // Hiển thị lỗi

            }
            else
            {
                // Kiểm tra phản hồi từ server
                if (www.downloadHandler.text.Contains("error"))
                {
                    errorMessages.text = "Tên đăng nhập hoặc mật khẩu không hợp lệ!";
                    Debug.Log("<color=red>" + www.downloadHandler.text + "</color>"); // Log lỗi từ server
                }
                else
                {
                    // Đăng nhập thành công
                    WebcomPanel.SetActive(true);
                    user.text = UserName.text;
                    Debug.Log("<color=green>Đăng nhập thành công: " + www.downloadHandler.text + "</color>");
                }
            }

            // Kích hoạt lại nút và ẩn vòng tròn tải
            LoginButton.interactable = true;
            progressCircle.SetActive(false);
        }
    }
    public void CallRegister() // goi panel register tu panel login
    {
        LoginPanel.SetActive(false);
        RegisterPanel.SetActive(true);

    }
    public void logOut()
    {
        WebcomPanel.SetActive(false );
        LoginPanel.SetActive(true );
        UserName.text = "";
        Password.text = "";
        errorMessages.text = "";
    }
    public void onRegisterButtoncliked() // xu ly nut dang ki 
    {
        RegisterButton.interactable = false; // Vô hiệu hóa nút login
        progressCircle.SetActive(true);  // Hiển thị vòng tròn tải
        if (string.IsNullOrEmpty(RegisterUserName.text) || string.IsNullOrEmpty(RegisterPassword.text) || string.IsNullOrEmpty(RegisterConfirmPassword.text))
        {
            RegisterErrorMessages.text = "Tên đăng nhập và mật khẩu không được để trống!";
            RegisterButton.interactable = true;
            progressCircle.SetActive(false);
            return; // Không tiếp tục gửi yêu cầu
        }
        if (RegisterPassword.text != RegisterConfirmPassword.text)
        {
            RegisterErrorMessages.text = "mat khau xac nhan khong khop";
            RegisterButton.interactable = true;
            progressCircle.SetActive(false);
            return;
        }
        if (RegisterUserName.text.Length < 3)
        {
            RegisterErrorMessages.text = "ten dang nhap khong duoc duoi 3 ki tu ";
            RegisterButton.interactable = true;
            progressCircle.SetActive(false);
            return;
        }
        if (RegisterPassword.text.Length < 6)
        {
            RegisterErrorMessages.text = "mat khau khong duoc duoi 6 ki tu ";
            RegisterButton.interactable = true;
            progressCircle.SetActive(false);
            return;
        }
        StartCoroutine(Register());
    }
    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", RegisterUserName.text);
        form.AddField("password", RegisterPassword.text);

        using (UnityWebRequest www = UnityWebRequest.Post(url + "/signup", form))
        {
            yield return www.SendWebRequest();

            if (www.error != null)
            {
                RegisterErrorMessages.text = "Tên đăng nhập đã tồn tại!";
                
            }
            else
            {
                // Kiểm tra phản hồi từ server
                if (www.downloadHandler.text.Contains("error"))
                {
                    RegisterErrorMessages.text = "Tên đăng nhập đã tồn tại!";
                    Debug.Log("<color=red>" + www.downloadHandler.text + "</color>");
                }
                else
                {
                    Debug.Log("<color=green>Đăng ký thành công: " + www.downloadHandler.text + "</color>");
                    // Chuyển về màn hình đăng nhập sau khi đăng ký thành công

                    SuccessPanel.SetActive(true);
                    StartCoroutine(ShowSuccessMessage());
                    
                    // Reset các trường input
                    RegisterUserName.text = "";
                    RegisterPassword.text = "";
                    RegisterConfirmPassword.text = "";
                    RegisterErrorMessages.text = "";
                }
            }

            RegisterButton.interactable = true;
            progressCircle.SetActive(false);
        }
    }
    public void BackToLogin()
    {
        RegisterPanel.SetActive(false);
        LoginPanel.SetActive(true);
        // Reset các trường input và thông báo lỗi
        RegisterUserName.text = "";
        RegisterPassword.text = "";
        RegisterConfirmPassword.text = "";
        RegisterErrorMessages.text = "";
    }
    IEnumerator ShowSuccessMessage()
    {
        // Đợi trong khoảng thời gian đã định
        yield return new WaitForSeconds(3f);

        // Ẩn panel thông báo
        SuccessPanel.SetActive(false);

        // Reset các trường input
        RegisterUserName.text = "";
        RegisterPassword.text = "";
        RegisterConfirmPassword.text = "";
        RegisterErrorMessages.text = "";

       
    }
}
