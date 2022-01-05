using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.GraphicRepresentation
{
    internal class GraphicController
    {
        private List<Point> _points;
        private int _width = 640;
        private int _height = 480;
        public int Scale;

        public GraphicController()
        {
            _points = new List<Point>();
            SeedPoint();
        }

        public void SetPointsInCenter()
        {
            foreach (var point in _points)
            {
                point.X = point.GetX() + _width / 2;
                point.Y = _height - (point.GetY() + _height / 2);

                point.X = point.X + Scale;
                point.Y = point.Y + Scale;
            }
        }

        public List<Point> GetPoints()
        {
            return _points;
        }

        private void SeedPoint()
        {
            _points.Add(new Point(0, 0, 20));
            _points.Add(new Point(100, 0));
            _points.Add(new Point(-100, 0));
            _points.Add(new Point(0, -100));
            _points.Add(new Point(0, 100));
            _points.Add(new Point(0, 200));
        }

        private void SeedSolarSystem()
        {
            // SolarSystem solarSystem = new();
        }
    }
}
