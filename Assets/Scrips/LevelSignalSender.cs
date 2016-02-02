using UnityEngine;
using System.Collections;

public class LevelSignalSender  {


	private static volatile LevelSignalSender instance;
	private static object syncRoot = new Object();
	
	private LevelSignalSender() {}
	
	public static LevelSignalSender Instance
	{
		get 
		{
			if (instance == null) 
			{
				lock (syncRoot) 
				{
					if (instance == null) 
						instance = new LevelSignalSender();
				}
			}
			
			return instance;
		}
	}

	public void signalLightTouched()
	{
		lock (syncRoot) 
		{
			Application.LoadLevel (0);
		}
	}
}
