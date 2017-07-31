using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AR.Story
{

	public class Manager : AR.Core.ManagerBase<AR.Story.Manager> {

		private int currentChapter = -1;

		private Dictionary<int, Chapter> chapters;

		public bool showSequenceSelectors;

		private bool ready = false;

		public void Awake () {
			chapters = new Dictionary<int, Chapter> ();
		}

		public void registerChapter (int _number, Chapter _chapter) {
			if (currentChapter == -1)
				currentChapter = 0;

			chapters.Add (_number, _chapter);
		}

		public void nextChapter() {
			if (currentChapter + 1 < chapters.Count) {
					gotoChapter(currentChapter+1);
			}
		}

		public void gotoChapter(int number)
		{
			if(number != currentChapter && number > -1 && number < chapters.Count) {
				Chapter seq = chapters[number];
                AR.Tracking.ViewPointManager.Instance.ResetViewPoint();
            }
		}

		public Chapter getCurrentChapter() {
			if(currentChapter > -1)
				return chapters[currentChapter];

			return null;
		}

		public List<Chapter> getChapters() {
			return new List<Chapter>(chapters.Values);
		}
	}

}