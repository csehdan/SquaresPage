﻿using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SquaresPage
{
	public partial class Default : System.Web.UI.Page
	{
		static List<Point> pts = new List<Point>();

		// auxiliary methods

		int? IsInt(string str)
		{
			int i=0;
			if (int.TryParse (str, out i))
				return i;
			else
				return null;
		}

		void EliminateRepeats()
		{
			if (pts.Count < 2)
				return;
			for (int i = 1; i < pts.Count; i++)
			{
				for (int j = 0; j < i; j++)
				{
					if (pts[i] == pts[j])
						pts.RemoveAt(i);
				}
			}
		}

		void Refresh()
		{
			EliminateRepeats();
			countLabel.Text = pts.Count.ToString();

			if (pts.Count > 0)
				btnExportList.Enabled = true;
			else
				btnExportList.Enabled = false;

			if (pts.Count > 3)
				btnCountSq.Enabled = true;
			else
				btnCountSq.Enabled = false;

			if (pts.Count > 0)
			{
				string pntLstStr = "Currently the following points are added:<br>";
				for (int i = 0; i < pts.Count; i++)
				{
					pntLstStr += pts[i].CoordsToString(true);
					pntLstStr += "<br>";
				}
				listOfPoints.Text = pntLstStr;
			}
			else
				listOfPoints.Text = "";

			listOfSquares.Text = "";
		}

		public void importFileList(bool del)
		{
			System.IO.Stream fileList = null;
			OpenFileDialog importListDialog = new OpenFileDialog();
			importListDialog.Filter = "All files (*.*)|*.*" ;
			importListDialog.RestoreDirectory = true ;

			if(importListDialog.ShowDialog() == DialogResult.OK)
			{
				try
				{
					if ((fileList = importListDialog.OpenFile()) != null)
					{
						List<Point> addedPts=new List<Point>();

						StreamReader sr = new StreamReader(fileList);
						String line=null;
						while(true)
						{
							line = sr.ReadLine();
							if(line == null) break;
							string[] twoNum = line.Split(new char[] {' ','\t'}, StringSplitOptions.RemoveEmptyEntries);

							string errortext="<script>alert('Incorrect data format in file. No action was carried out.');</script>";

							if(twoNum.Length!=2)
							{
								Response.Write (errortext);
								return;
							}
							int? x=IsInt(twoNum[0]);
							int? y=IsInt(twoNum[1]);
							if(x==null || y==null)
							{
								Response.Write (errortext);
								return;
							}

							addedPts.Add(new Point(x.Value, y.Value));
						}

						if(del)
						{
							// ezt csinaljuk csak akkor, ha minden tuti, sikerult beolvasni a fajlt
							pts.Clear();
						}

						pts.AddRange(addedPts);
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
				}
				return;
			}
		}

		// btnClicked methods

		public void btnAddPtClicked(object sender, EventArgs args)
		{
			int? Xc=IsInt(XCoord.Value);
			int? Yc=IsInt(YCoord.Value);

			if (Xc.HasValue && Yc.HasValue)
			{
				Point newp = new Point(Xc.Value, Yc.Value);
				bool notyet = true; // was not requested, but makes no sense to store identical points
				for (int i = 0; i < pts.Count; i++)
				{
					if (pts[i] == newp)
					{
						Response.Write ("<script>alert('This point already exists in the list, it will not be added again.');</script>");
						notyet = false;
						break;
					}
				}

				XCoord.Value = "";
				YCoord.Value = "";

				if (notyet)
					pts.Add(newp);
			}
			else
			{
				Response.Write ("<script>alert('Please provide an integer numbers as coordinates!');</script>");
				if (!Xc.HasValue)
					XCoord.Value = "";
				if (!Yc.HasValue)
					YCoord.Value = "";
			}
			Refresh();
		}

		public void btnClearListClicked(object sender, EventArgs args)
		{
			pts.Clear();
			Refresh();
		}

		public void btnImportListClicked(object sender, EventArgs args)
		{
			DialogResult result = MessageBox.Show("Do you want the existing points to be removed? " +
				"If not, points from the file will be added to the already existing points.",
				"Delete existing points?",
				MessageBoxButtons.YesNoCancel,
				MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button1);

			if (result == DialogResult.Yes)
			{
				importFileList(true);
			}
			else if (result == DialogResult.No)
			{
				importFileList(false);
			}
			Refresh();
			return;
		}

		public void btnExportListClicked(object sender, EventArgs args)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Title = "Export points to file";
			DialogResult dlgRes = sfd.ShowDialog();
			if (dlgRes == DialogResult.OK && sfd.FileName != "")
			{
				System.IO.FileStream fs = (System.IO.FileStream)sfd.OpenFile();
				StreamWriter sw = new StreamWriter(fs);
				sw.BaseStream.Seek(0, SeekOrigin.End);

				for (int i = 0; i < pts.Count; i++)
				{
					sw.Write(pts[i].CoordsToString()+"\n");
				}

				sw.Close();
			}
		}

		public void btnCountSqClicked(object sender, EventArgs args)
		{
			int countSq = 0;
			Qdrs qts = new Qdrs(pts);
			for (int i = 0; i < qts.QS.Count; i++)
			{
				if (qts.QS[i].IsSquare())
				{
					countSq++;
					if (countSq == 1)
					{
						listOfSquares.Text = "Squares are the following:<br>";
					}
					listOfSquares.Text += qts.QS[i].GetPointsText()+"<br>";
				}

			}
			string msg = "Number of squares: " + countSq;
			Response.Write ("<script>alert('" +msg+ "');</script>");
		}
	}
}
