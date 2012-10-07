using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TitlePage : FContainer, FMultiTouchableInterface {
	
	FLabel title;
	
	public TitlePage() {
		FSprite background = new FSprite("whiteSquare.png");
		background.x = Futile.screen.halfWidth;
		background.y = Futile.screen.halfHeight;
		background.width = Futile.screen.width;
		background.height = Futile.screen.height;
		AddChild(background);
		
		title = new FLabel("BlairMdITC", "Catch Me!");
		title.color = Color.black;
		title.scale = 0f;
		title.x = Futile.screen.halfWidth;
		title.y = Futile.screen.halfHeight;
		AddChild(title);
		
		Go.to(title, 1.0f, new TweenConfig().addTweenProperty(new FloatTweenProperty("scale", 1.0f, false)).setEaseType(EaseType.BackInOut).onComplete(HandleTitleDoneInflating));
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
	
	public void HandleTitleDoneInflating(AbstractTween abstractTween) {
		/*TweenConfig alphaTweenConfig = new TweenConfig();

		TweenConfig scaleUpTweenConfig = new TweenConfig();
		scaleUpTweenConfig.addTweenProperty(new FloatTweenProperty("scale", 1.1f, false));
		scaleUpTweenConfig.setEaseType(EaseType.BackOut);
		
		TweenConfig scaleDownTweenConfig = new TweenConfig();
		scaleUpTweenConfig.addTweenProperty(new FloatTweenProperty("scale", 1.0f, false));
		scaleUpTweenConfig.setEaseType(EaseType.BackOut);
		
		Tween scaleUpTween = new Tween(this, 0.2f, scaleUpTweenConfig);
		Tween scaleDownTween = new Tween(this, 0.2f, scaleDownTweenConfig);
				
		TweenFlow tweenFlow = new TweenFlow();
		tweenFlow.insert(0.0f, scaleUpTween);
		tweenFlow.insert(0.2f, scaleDownTween);
		tweenFlow.setIterations(10000000, LoopType.RestartFromBeginning);
		tweenFlow.play();*/
	}
	
	public void HandleUpdate() {
		
	}
	
	public void HandleMultiTouch(FTouch[] touches) {		
		foreach (FTouch touch in touches) {
			if (touch.phase == TouchPhase.Began) Main.SwitchToPage(Main.PageType.Game);
		}
	}
}