using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using minesweeper;
using System.Collections.Generic;

namespace Tests
{
    
    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public void shouldreturnPoint()
        {
            //Arrange
            Game g = new Game();
            List<Point> pointsGenerated = new List<Point>();
            g.MinesCoordinates= new List<Point>();
            Point p1 = new Point(0, 0);
            Point p2 = new Point(0, 1);

            pointsGenerated.Add(p1);
            pointsGenerated.Add(p2);

            Random r = new Random();

            //Act
            Point result = g.GetRandomPoint(r, pointsGenerated);

            //Assert 
            Assert.IsInstanceOfType(result, typeof(Point));
          
        }
    }
}
