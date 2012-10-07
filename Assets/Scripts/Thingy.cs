using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Thingy : FContainer {
	public float rotationSpeed = Random.Range(50, 500);
	public float xVelocity = Random.Range(50, 200);
	public float yVelocity = Random.Range(50, 200);
	private bool isGood_ = true;

	public Thingy() : this(true) {
		
	}
	
	public Thingy(bool isGood) {
		isGood_ = isGood;
				
		if (isGood) {
			FSprite thingy = new FSprite("me.png");
						
			/*FSprite rectSprite = new FSprite("whiteSquare.png");
			rectSprite.width = heart.width;
			rectSprite.height = heart.height;
			rectSprite.color = Color.blue;
			rectSprite.alpha = 0.3f;
			AddChild(rectSprite);*/
				
			AddChild(thingy);
		}
		else {
			FSprite thingy = new FSprite("whiteSquare.png");
			thingy.width = 50;
			thingy.height = 100;
				
			thingy.color = new Color(1.0f, Random.Range (0, 100) / 255.0f, Random.Range(0, 100) / 255.0f, 1.0f);
			
			AddChild(thingy);
			
			/*FSprite left = new FSprite("brokenHeartLeft.png");
			FSprite right = new FSprite("brokenHeartRight.png");
			AddChild(left);
			AddChild(right);*/
		}
		
		if (Random.Range(0, 2) == 0) rotationSpeed *= -1;
		if (Random.Range(0, 2) == 0) xVelocity *= -1;
		if (Random.Range(0, 2) == 0) yVelocity *= -1;
		
		scale = 0;
		rotation = Random.Range(0, 360);
	}
	
	override public void HandleAddedToStage() {
		Futile.instance.SignalUpdate += HandleUpdate;
		base.HandleAddedToStage();
	}	
	
	override public void HandleRemovedFromStage() {
		Futile.instance.SignalUpdate -= HandleUpdate;
		base.HandleAddedToStage();
	}
	
	public void Inflate() {
		float newScale = RXRandom.Range(0.2f, 0.6f);
		if (isGood_) newScale = 0.6f;
		
		Go.to(this, 0.3f, new TweenConfig().addTweenProperty(new FloatTweenProperty("scale", newScale, false)).setEaseType(EaseType.BackOut));
	}
	
	public bool isGood {
		get {return isGood_;}
	}

	public void HandleUpdate() {
		rotation += rotationSpeed * Time.deltaTime;
	}
	
	public void Destroy() {
		TweenConfig alphaTweenConfig = new TweenConfig();
		alphaTweenConfig.addTweenProperty(new FloatTweenProperty("alpha", 0.0f, false));

		TweenConfig scaleTweenConfig = new TweenConfig();
		scaleTweenConfig.addTweenProperty(new FloatTweenProperty("scale", 20, false));
		
		Tween alphaTween = new Tween(this, 0.3f, alphaTweenConfig);
		Tween scaleTween = new Tween(this, 0.3f, scaleTweenConfig);
				
		TweenFlow tweenFlow = new TweenFlow();
		tweenFlow.insert(0.0f, alphaTween);
		tweenFlow.insert(0.0f, scaleTween);
		tweenFlow.setOnCompleteHandler(DoneDestroying);
		tweenFlow.play();
	}
	
	public void DoneDestroying(AbstractTween tween) {
		for (int i = _childNodes.Count - 1; i >= 0; i--) {
			FSprite sprite = GetChildAt(0) as FSprite;
			RemoveChild(sprite);
		}
		this.RemoveFromContainer();	
	}
}