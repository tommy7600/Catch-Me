using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GamePage : FContainer, FMultiTouchableInterface {
	private List <Thingy> thingies = new List<Thingy>();
	private int maxThingies = 2;
	private FContainer thingyContainer = new FContainer();
	private int maxGoodThingies = 1;
	private FButton again;
	private int goodThingiesCount = 0;
	private bool gameOver = false;
	private HUDLayer hudLayer = new HUDLayer();
	private Rect playAreaRect = new Rect(0.0f, Futile.screen.height - 30, Futile.screen.width, Futile.screen.height - 30);	
	static float timer = 0;

	public GamePage() {
		FSprite background = new FSprite("whiteSquare.png");
		background.x = Futile.screen.halfWidth;
		background.y = Futile.screen.halfHeight;
		background.width = Futile.screen.width;
		background.height = Futile.screen.height;
		AddChild(background);
		
		AddChild(thingyContainer);
		
		AddChild(hudLayer);
		
		InitThingies();
	}

	public void InitThingies ()
	{
		for (int i = 0; i < maxThingies; i++) {
			AddNewThingy(false);
		}
	}
	
	override public void HandleAddedToStage() {
		Futile.touchManager.AddMultiTouchTarget(this);
		Futile.instance.SignalUpdate += HandleUpdate;
		base.HandleAddedToStage();
	}	
	
	override public void HandleRemovedFromStage() {
		Futile.touchManager.RemoveMultiTouchTarget(this);
		Futile.instance.SignalUpdate -= HandleUpdate;
		base.HandleAddedToStage();
	}
	
	private void AddNewThingy(bool withSound) {
		Thingy thingy;
		
		if (withSound) FSoundManager.PlaySound("spawn");
		
		if (goodThingiesCount < maxGoodThingies) {
			thingy = new Thingy(true);
			goodThingiesCount++;
		}
		else {
			thingy = new Thingy(false);	
		}
		thingy.x = Random.Range(thingy.GetContentRect().width / 2, Futile.screen.width - thingy.GetContentRect().width / 2);
		thingy.y = Random.Range(thingy.GetContentRect().height / 2, Futile.screen.height - thingy.GetContentRect().height / 2);
		thingyContainer.AddChild(thingy);
		thingies.Add(thingy);
		thingy.Inflate();
	}
	
	private void MoveGoodThingiesToBottom() {
		foreach(Thingy thingy in thingies) {
			if (thingy.isGood) {
				thingyContainer.AddChildAtIndex(thingy, 0);	
			}
		}
	}
	
	private void HandleUpdate () {
		if (!gameOver) {
			UpdateThingyPositions();
			
			timer += Time.deltaTime;
			
			if (thingies.Count < maxThingies && timer >= RXRandom.Range(0.1f, 0.5f)) {
				timer = 0;
				AddNewThingy(true);
				MoveGoodThingiesToBottom();
			}
		}
	}
	
	private void UpdateThingyPositions() {
		foreach (Thingy thingy in thingies) {						
			if (thingy.isGood) {
				int maxVelocity = 250;
				
				if (RXRandom.Float() < 0.2f) thingy.xVelocity += RXRandom.Range(-100, 100);	
				if (RXRandom.Float() < 0.2f) thingy.yVelocity += RXRandom.Range(-100, 100);
				
				if (thingy.xVelocity > maxVelocity) thingy.xVelocity = maxVelocity;
				if (thingy.xVelocity < -maxVelocity) thingy.xVelocity = -maxVelocity;
				
				if (thingy.yVelocity > maxVelocity) thingy.yVelocity = maxVelocity;
				if (thingy.yVelocity < -maxVelocity) thingy.yVelocity = -maxVelocity;
			}
			
			float deltaX = thingy.xVelocity * Time.deltaTime;
			float deltaY = thingy.yVelocity * Time.deltaTime;
			
			FSprite sprite = thingy.GetChildAt(0) as FSprite;
			Vector2 maxes = sprite.GetGlobalTextureRectMaxes();
			Vector2 mins = sprite.GetGlobalTextureRectMins();
			
			if (maxes.x + deltaX > playAreaRect.width) {
				deltaX = playAreaRect.width - maxes.x;
				thingy.xVelocity *= -1;
			}
			else if (mins.x + deltaX < 0) {
				deltaX = -mins.x;
				thingy.xVelocity *= -1;
			}
			
			if (maxes.y + deltaY > playAreaRect.height) {
				deltaY = playAreaRect.height - maxes.y;
				thingy.yVelocity *= -1;
			}
			else if (mins.y + deltaY < 0) {
				deltaY = -mins.y;
				thingy.yVelocity *= -1;
			}
			
			thingy.x += deltaX;
			thingy.y += deltaY;
		}
	}
	
	public void HandleMultiTouch(FTouch[] touches) {
		foreach(FTouch touch in touches) {
			if (touch.phase == TouchPhase.Began) {
				for (int i = thingies.Count - 1; i >= 0; i--) {
					Thingy thingy = thingies[i];
					FSprite sprite = thingy.GetChildAt(0) as FSprite;
					Vector2 touchPos = sprite.GlobalToLocal(touch.position);
					Rect rect = sprite.textureRect;
					
					if (rect.Contains(touchPos)) {
						HandleGotThingy(thingy);
						break;
					}
				}
			}
		}
	}
	
	private void HandleGotThingy(Thingy thingy) {
		if (thingy.isGood) {
			FSoundManager.PlaySound("get");
			hudLayer.score += 1;
			maxThingies += 5;
			goodThingiesCount--;
		}
		else {
			gameOver = true;
			FSoundManager.PlaySound("lose");

			FLabel score = new FLabel("BlairMdITC", hudLayer.score.ToString());
			score.x = Futile.screen.halfWidth;
			score.y = Futile.screen.halfHeight + 75;
			score.scale = 0f;
			score.color = Color.black;
			Go.to(score, 0.5f, new TweenConfig().addTweenProperty(new FloatTweenProperty("scale", 1.0f, false)).setEaseType(EaseType.BackInOut));
			
			AddChild(score);
			
			again = new FButton("button.png", "buttonOver.png", "spawn");
			again.SignalRelease += HandleAgainSignalRelease;
			again.AddLabel("BlairMdITC", "Play Again", Color.black);
			again.label.scale = 0.3f;
			again.scale = 0f;
			again.x = Futile.screen.halfWidth;
			again.y = Futile.screen.halfHeight;
			Go.to(again, 0.5f, new TweenConfig().addTweenProperty(new FloatTweenProperty("scale", 1.0f, false)).setEaseType(EaseType.BackInOut));
			
			AddChild(again);
		}
		
		thingies.Remove(thingy);
		thingy.Destroy();
	}

	void HandleAgainSignalRelease (FButton obj)
	{
		again.RemoveFromContainer();
		Main.SwitchToPage(Main.PageType.Game);
	}
}
