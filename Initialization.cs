using System;
using Unity.Services.Core;
using UnityEngine;

// Inicializando serviços unity - para efetuar login pela api da unity (google, facebook, guest...)
// usado no GameObject Initializator

public class Initialization : MonoBehaviour
{
	async void Awake()
	{
		try
		{
			await UnityServices.InitializeAsync();
			Debug.Log("Inicializou");
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}
	}
}