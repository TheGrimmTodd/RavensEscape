using UnityEngine;
using System.Collections;

public class LevelSignalSender  {


	private static volatile LevelSignalSender instance;
	private static object syncRoot = new Object();
	private System.Collections.ArrayList lightListeners;
	private System.Collections.ArrayList endListeners;

	
	private LevelSignalSender() {
		lightListeners = new System.Collections.ArrayList ();
		endListeners = new System.Collections.ArrayList ();
		
	}
	
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

	/*Add Listeners*/
	public void addLightTouchedListener(OnLightTouchedListener listener){
		lock (syncRoot) 
		{
			lightListeners.Add (listener);
		}
	}
	public void addEndZonetTouchedListener(OnEndZoneTouchedListener listener){
		lock (syncRoot) 
		{
			endListeners.Add (listener);
		}
	}
	/*Remove Listeners*/
	public void removeLightTouchedListener(OnLightTouchedListener listener){
		lock (syncRoot) 
		{
			lightListeners.Remove (listener);
		}
	}
	public void removeEndZoneTouchedListener(OnEndZoneTouchedListener listener){
		lock (syncRoot) 
		{
			endListeners.Remove (listener);
		}
	}








	/*Signals*/
	public void signalLightTouched()
	{
		lock (syncRoot) 
		{
			foreach( object obj in lightListeners)
				if (obj.GetType().Equals(typeof(OnLightTouchedListener)))
					((OnLightTouchedListener) obj).OnTouch();
			//TODO: handle correctly
			Application.LoadLevel (0);
		}
	}
	public void signalEndZoneTouched()
	{
		lock (syncRoot) 
		{
			foreach( object obj in lightListeners)
				if (obj.GetType().Equals(typeof(OnEndZoneTouchedListener)))
					((OnEndZoneTouchedListener) obj).OnTouch();
			//TODO: handle correctly
			Application.LoadLevel (0);
		}
	}




	/*Iterfaces*/
	public interface OnLightTouchedListener{
		void OnTouch ();
	}
	public interface OnEndZoneTouchedListener{
		void OnTouch();
	}


}
