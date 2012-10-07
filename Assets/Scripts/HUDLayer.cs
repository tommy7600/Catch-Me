using UnityEngine;
using System.Collections;

public class HUDLayer : FContainer {
	
	private int score_;
	private bool isDirty = false;
	private FLabel scoreLabel = new FLabel("BlairMdITC", "0");
	
	public HUDLayer() : base() {
		FSprite banner = new FSprite("whiteSquare.png");
		banner.width = Futile.screen.width;
		banner.height = 30;
		banner.x = Futile.screen.halfWidth;
		banner.anchorY = 0;
		banner.y = Futile.screen.height - 30;
		banner.color = new Color(0.25f, 0.25f, 0.25f, 1.0f);
		AddChild(banner);
		
		scoreLabel.x = Futile.screen.halfWidth;
		scoreLabel.y = Futile.screen.height - 15;
		scoreLabel.scale = 0.3f;
		AddChild(scoreLabel);
	}
	
	override public void HandleAddedToStage() {
		Futile.instance.SignalUpdate += HandleUpdate;
		base.HandleAddedToStage();
	}	
	
	override public void HandleRemovedFromStage() {
		Futile.instance.SignalUpdate -= HandleUpdate;
		base.HandleAddedToStage();
	}	
	
	public void HandleUpdate() {
		if (isDirty) {
			scoreLabel.text = score.ToString();
		}
	}
	
	public int score {
		get {return score_;}
		set {
			score_ = value;
			isDirty = true;
		}		
	}
}
