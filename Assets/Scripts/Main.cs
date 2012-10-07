using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	
	public static FContainer currentPage;
	
	public enum PageType {
		None,
		Title,
		Game
	}
	
	// Use this for initialization
	void Start () {
		FutileParams fparams = new FutileParams(true, true, false, false);
		fparams.AddResolutionLevel(480.0f, 1.0f, 1.0f, "");
		fparams.origin = new Vector2(0.0f, 0.0f);
		
		Futile.instance.Init(fparams);
		Futile.atlasManager.LoadAtlas("Atlases/mainAtlas");
		Futile.atlasManager.LoadAtlas("Atlases/backgroundAtlas");
		Futile.atlasManager.LoadFont("BlairMdITC", "BlairMdITC.png", "Atlases/BlairMdITC");
		
		FSoundManager.PlayMusic("song", 0.7f);
		
		SwitchToPage(PageType.Title);
	}

	public static void SwitchToPage(PageType pageType) {
		if (currentPage != null) currentPage.RemoveFromContainer();
		
		if (pageType == PageType.Title) currentPage = new TitlePage();
		else if (pageType == PageType.Game) currentPage = new GamePage();
		
		currentPage.x = 0;
		currentPage.y = 0;
		Futile.stage.AddChild(currentPage);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
