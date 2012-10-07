using System;
using UnityEngine;
using System.Collections.Generic;

public static class FContainerExtensions
{
	public static Rect GetContentRect(this FContainer container)
	{
		int childCount = container.GetChildCount();
		if (childCount == 0) return new Rect(0, 0, 0, 0);
		
		float minX = 1000000f;
		float maxX = -1000000f;
		float minY = 1000000f;
		float maxY = -1000000f;
		
		for (int i = 0; i < childCount; i++) {
			FSprite sprite = container.GetChildAt(i) as FSprite;
			
			float spriteMinX = sprite.textureRect.xMin + (sprite.textureRect.width - (sprite.scaleX * sprite.textureRect.width)) / 2;
			float spriteMaxX = sprite.textureRect.xMax - (sprite.textureRect.width - (sprite.scaleX * sprite.textureRect.width)) / 2;
			float spriteMinY = sprite.textureRect.yMin + (sprite.textureRect.height - (sprite.scaleX * sprite.textureRect.width)) / 2;
			float spriteMaxY = sprite.textureRect.yMax - (sprite.textureRect.height - (sprite.scaleX * sprite.textureRect.width)) / 2;
			
			minX = Mathf.Min(minX, spriteMinX);
			maxX = Mathf.Max(maxX, spriteMaxX);
			minY = Mathf.Min(minY, spriteMinY);
			maxY = Mathf.Max(maxY, spriteMaxY);
		}
		
		float width = maxX - minX;
		float height = maxY - minY;
		
		minX = minX + (width - (container.scaleX * width) / 2);
		maxX = maxX + (width - (container.scaleX * width) / 2);
		minY = minY + (height - (container.scaleY * height) / 2);
		maxY = maxY + (height - (container.scaleY * height) / 2);
		
		width = container.scaleX * width;
		height = container.scaleY * height;
		
		Rect rect = new Rect(minX, maxY, width, height);
				
		return rect;
	}
}

public static class RectExtensions
{
	public static Vector2[] GetCoordinates(this Rect rect)
	{
		Vector2[] vectors = new Vector2[4];
		
		vectors[0] = new Vector2(rect.x, rect.y);
		vectors[1] = new Vector2(rect.x + rect.width, rect.y);
		vectors[2] = new Vector2(rect.x, rect.y + rect.height);
		vectors[3] = new Vector2(rect.x + rect.width, rect.y + rect.height);
		
		return vectors;
	}
}

public static class FSpriteExtensions
{
	public static Vector2 GetGlobalTextureRectMins(this FSprite sprite)
	{
		Vector2[] vectors = sprite.textureRect.GetCoordinates();
		
		vectors[0] = sprite.LocalToGlobal(vectors[0]);
		vectors[1] = sprite.LocalToGlobal(vectors[1]);
		vectors[2] = sprite.LocalToGlobal(vectors[2]);
		vectors[3] = sprite.LocalToGlobal(vectors[3]);
				
		float minX = 1000000;
		float minY = 1000000;
					
		minX = Mathf.Min(minX, vectors[0].x);
		minX = Mathf.Min(minX, vectors[1].x);
		minX = Mathf.Min(minX, vectors[2].x);
		minX = Mathf.Min(minX, vectors[3].x);
				
		minY = Mathf.Min(minY, vectors[0].y);
		minY = Mathf.Min(minY, vectors[1].y);
		minY = Mathf.Min(minY, vectors[2].y);
		minY = Mathf.Min(minY, vectors[3].y);
		
		return new Vector2(minX, minY);
	}
	
	public static Vector2 GetGlobalTextureRectMaxes(this FSprite sprite)
	{
		Vector2[] vectors = sprite.textureRect.GetCoordinates();
		
		vectors[0] = sprite.LocalToGlobal(vectors[0]);
		vectors[1] = sprite.LocalToGlobal(vectors[1]);
		vectors[2] = sprite.LocalToGlobal(vectors[2]);
		vectors[3] = sprite.LocalToGlobal(vectors[3]);
				
		float maxX = -1000000;
		float maxY = -1000000;
					
		maxX = Mathf.Max(maxX, vectors[0].x);
		maxX = Mathf.Max(maxX, vectors[1].x);
		maxX = Mathf.Max(maxX, vectors[2].x);
		maxX = Mathf.Max(maxX, vectors[3].x);
				
		maxY = Mathf.Max(maxY, vectors[0].y);
		maxY = Mathf.Max(maxY, vectors[1].y);
		maxY = Mathf.Max(maxY, vectors[2].y);
		maxY = Mathf.Max(maxY, vectors[3].y);
		
		return new Vector2(maxX, maxY);
	}
}