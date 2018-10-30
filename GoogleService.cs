using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoogleService : MonoBehaviour
{

	public PictureFactory pictureFactory;
	public Text buttonText;
	private const string API_KEY = "AIzaSyBS9WtHHhsFK6Z9KWB_hmZE48gieoJomuI"; 
	public void GetPicture()
	{
		StartCoroutine(PictureRoutine());
	}

	IEnumerator PictureRoutine()
	{
		buttonText.transform.parent.gameObject.SetActive(false);
		string query = buttonText.text;
		query = WWW.EscapeURL(query);
		pictureFactory.DeleteOldPictures();
		Vector3 cameraForward = Camera.main.transform.forward;

		int rowNum = 1;
		for(int i=1; i<=60; i+= 5)
		{
			string url = "https://www.googleapis.com/customsearch/v1?q=pyramids&cx=000840476634705069223%3Arw0zula5h_0&filter=1&num=10&searchType=image&start="+i+"1&fields=items%2Flink&key="+ API_KEY;

			WWW www = new WWW (url);
			yield return www;
			pictureFactory.CreateImages(ParseResponse(www.text), rowNum, cameraForward);
			rowNum++;

		}
		yield return new WaitForSeconds(5f);
		buttonText.transform.parent.gameObject.SetActive(true);
	}
	List<string> ParseResponse(string text)
	{
		List<string> urlList = new List<string> ();
		string[] urls = text.Split('\n');
		foreach (string line in urls)
		{
			if (line.Contains ("link")) {
				string url = line.Substring (12, line.Length - 13);
				if (url.Contains (".jpg") || url.Contains (".png")) {
					urlList.Add (url);
				}
			}
		}
		return urlList;
	}


}
