using System.Collections;
using Firebase;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace LoginScene {
    public class DB : MonoBehaviour, IDataBase {
        public FirebaseAuth auth;
        public FirebaseUser User;

        void Start() {
            auth = FirebaseAuth.DefaultInstance;
        }

        public IEnumerator Login(string email, string password) {
            //Debug.Log($"{email} and {password}");
            var LoginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

            if (LoginTask.Exception != null) {
                //If there are errors handle them
                FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError) firebaseEx.ErrorCode;

                string message = "Login Failed!";
                switch (errorCode) {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        Debug.Log(message);
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        Debug.Log(message);
                        break;
                    case AuthError.WrongPassword:
                        message = "Wrong Password";
                        Debug.Log(message);
                        break;
                    case AuthError.InvalidEmail:
                        message = "Invalid Email";
                        Debug.Log(message);
                        break;
                    case AuthError.UserNotFound:
                        message = "Account does not exist";
                        Debug.Log(message);
                        break;
                }
            }
            else {
                //User is now logged in
                //Now get the result
                User = LoginTask.Result;
                UserData.UserID = User.UserId;
                SceneManager.LoadScene("MainScene");
            }
        }

        public string GetFacebookToken() {
            return "ewjk";
        }

        public IEnumerator SignUp(string email, string password) {
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null) {
                //If there are errors handle them
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError) firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode) {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        Debug.Log(message);
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        Debug.Log(message);
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        Debug.Log(message);
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        Debug.Log(message);
                        break;
                }
            }
            else {
                //User has now been created
                //Now get the result
                User = RegisterTask.Result;
                UserData.UserID = User.UserId;
                SceneManager.LoadScene("MainScene");
            }
        }
    }
}


/*public void SignUp(string username, string password) {
    Debug.Log($"{username} and {password} signup");
}*/