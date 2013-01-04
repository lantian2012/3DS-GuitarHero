using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GuitarHeroController;
using GuitarHeroModel;
using System.IO;
using System.Collections.Generic;

namespace GuitarHeroUnitTest
{
    [TestClass]
    public class CLogicTest
    {
        /// <summary>
        /// If hands are staying in the hitting zone,
        /// check score counting and scorability
        /// </summary>
        /// <remarks>
        /// 1. note in string[0], left hand stays-> not scorable
        /// 2. note in string[2], left hand stays-> not scorable
        /// 3. note in string[1], no hand->scoralbe
        /// </remarks>
        [TestMethod]
        public void Clogic_handstay()
        {
            MOptions moptions = new MOptions();
            moptions.loadOptionFile();
            CLogic clogic = new CLogic(moptions);
            clogic.addNewNote(0);
            clogic.RightShadow.X = 0;
            clogic.RightShadow.Y = 0;
            clogic.LeftShadow.X = 50;
            clogic.LeftShadow.Y = 600;
            PrivateObject cLogicPri = new PrivateObject(clogic);
            PrivateObject cNotePri = new PrivateObject((CNote)cLogicPri.GetFieldOrProperty("_CNote"));
            List<CString> cString = (List<CString>)cNotePri.GetFieldOrProperty("Strings");
            PrivateObject cStringPri = new PrivateObject(cString[0]);
            List<Note> note = (List<Note>)cStringPri.GetFieldOrProperty("notes");
            PrivateObject notePri = new PrivateObject(note[0]);
            
            while ((int)notePri.GetFieldOrProperty("_Y") < 766)
            {
                clogic.checkString();
                clogic.advanceFrame();
            }
            Assert.IsFalse(note[0].isScorable);
            Assert.AreEqual(0, clogic.Score);

            clogic.addNewNote(2);
            clogic.RightShadow.X = 700;
            clogic.RightShadow.Y = 590;
            cStringPri = new PrivateObject(cString[2]);
            note = (List<Note>)cStringPri.GetFieldOrProperty("notes"); 
            while ((int)note[0].Y < 766)
            {
                clogic.checkString();
                clogic.advanceFrame();
            }
            Assert.IsFalse(note[0].isScorable);
            Assert.AreEqual(0, clogic.Score);
            clogic.addNewNote(1);
            cStringPri = new PrivateObject(cString[1]);
            note = (List<Note>)cStringPri.GetFieldOrProperty("notes");
            while ((int)note[0].Y < 766)
            {
                clogic.checkString();
                clogic.advanceFrame();
            }
            Assert.IsFalse(!note[0].isScorable);
            Assert.AreEqual(0, clogic.Score);

        }

        /// <summary>
        /// when hit at right time, score added?
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        [TestMethod]
        public void Clogic_hit()
        {
            MOptions moptions = new MOptions();
            moptions.loadOptionFile();
            CLogic clogic = new CLogic(moptions);
            clogic.addNewNote(0);
            PrivateObject cLogicPri = new PrivateObject(clogic);
            PrivateObject cNotePri = new PrivateObject((CNote)cLogicPri.GetFieldOrProperty("_CNote"));
            List<CString> cString = (List<CString>)cNotePri.GetFieldOrProperty("Strings");
            PrivateObject cStringPri = new PrivateObject(cString[0]);
            List<Note> note = (List<Note>)cStringPri.GetFieldOrProperty("notes");
            while (note[0].Y < 768)
            {
                clogic.checkString();
                clogic.advanceFrame();
                if (note[0].Y > 580)
                {
                    if (note[0].Y > 610)
                    {
                        clogic.LeftShadow.X = 159;
                        clogic.LeftShadow.Y = 580;
                    }
                }
            }
            Assert.AreEqual(clogic.Score, 1);
            Assert.IsFalse(note[0].isAlive);

            clogic.addNewNote(1);
            cStringPri = new PrivateObject(cString[1]);
            note = (List<Note>)cStringPri.GetFieldOrProperty("notes");
            while ((int)note[0].Y < 766)
            {
                clogic.checkString();
                clogic.advanceFrame();
                if (note[0].Y > 580)
                {
                    if (note[0].Y > 610)
                    {
                        clogic.LeftShadow.X = 340;
                        clogic.LeftShadow.Y = 580;
                    }
                }
            }
            Assert.AreEqual(clogic.Score, 2);
            Assert.IsFalse(note[0].isAlive);

            clogic.addNewNote(2);
            cStringPri = new PrivateObject(cString[2]);
            note = (List<Note>)cStringPri.GetFieldOrProperty("notes");
            while ((int)note[0].Y < 766)
            {
                clogic.checkString();
                clogic.advanceFrame();
                if (note[0].Y > 580)
                {
                    if (note[0].Y > 610)
                    {
                        clogic.LeftShadow.X = 780;
                        clogic.LeftShadow.Y = 580;
                    }
                }
            }
            Assert.AreEqual(clogic.Score, 3);
            Assert.IsFalse(note[0].isAlive);
        }

    }
}
