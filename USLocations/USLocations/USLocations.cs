/*
 * Wendy Thach
 * I have successfully completed all requirements to this project.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
public class USLocations
{
	private Task<List<Location>> inputTask;         // Task for reading file
	private List<Location> locations;   // Read locations info here
										// This constructor will initiate the loading of the TSV file.
										// The constructor must return very quickly, perhaps before all 
										// of the zip code information has been completely loaded. Tasks
										// will be needed to do this.
	public USLocations(string fileName)
	{
		inputTask = ReadFile(fileName);
		inputTask.Wait();
		locations = inputTask.Result;
	}

	public class Location
    {
		public string zipcode;
		public string city;
		public string state;
		public string latitude;
		public string longitude;

		public Location(string zipcode, string city, string state, string latitude, string longitude)
        {
			this.zipcode = zipcode;
			this.city = city;
			this.state = state;
			this.latitude = latitude;
			this.longitude = longitude;
        }
    }

	public async Task<List<Location>> ReadFile(string filename)   // Asynchronous method for reading file
	{
        //Reads in file and returns a list of only the zipcode, city, state, latitiude, and longitude from each line
		StreamReader reader = new StreamReader(filename);
		List<Location> l = new List<Location>();
		reader.ReadLine();
		while (!reader.EndOfStream)
		{
			string line = await reader.ReadLineAsync();
			string[] toks = line.Split('\t');
			l.Add(new Location(toks[1], toks[3], toks[4], toks[6], toks[7]));
		}
        reader.Close();
		return l;
	}
    /**
        * Returns the city names that appear in both of the given states.
            * "OH" and "MI" would yield {OXFORD, FRANKLIN, ... }
        */
    public ISet<string> GetCommonCityNames(string state1, string state2)
    {
        // Cycle through locations to find cities common to both states.
        // Before doing this, you may need to wait for the reading to complete.
        ISet<string> result = new SortedSet<string>();
        List<string> s1 = new List<string>();
        List<string> s2 = new List<string>();

        // Loops through the locations list and adds the first and second state's cities into s1 and s2
        foreach (Location l in locations)
        {
            if (l.state.Equals(state1))
            {
                s1.Add(l.city);
            }
            if (l.state.Equals(state2))
            {
                s2.Add(l.city);
            }
        }

        // Loops through the 2 lists and checks if their cities are equal then adds them to result
        foreach (string city1 in s1)
        {
            foreach (string city2 in s2)
            {
                if (city1.Equals(city2))
                {
                    result.Add(city1);
                }
            }
        }

        return result;
    }
    /**
    * Returns all zipcodes that are within a specified distance from a
    * particular zipcode.
    */
    // Do this by researching the "Haversine" formula to do this one.
    // The formula for converting from degrees to radians is:
    //     radians = degrees * Math.PI / 180.0;
    public double Distance(int zip1, int zip2)
    {
        // Use Haversine to compute distance between two locations.
        // Before doing this, you may need to wait for the reading to complete.
        double latitude1 = 0;
        double latitude2 = 0;
        double longitude1 = 0;
        double longitude2 = 0;

        // Loops through the locations list and sets the variables to the zipcode
        foreach (Location l in locations)
        {
            if (zip1 == Int32.Parse(l.zipcode))
            {
                latitude1 = Double.Parse(l.latitude);
                longitude1 = Double.Parse(l.longitude);
            }
            if (zip2 == Int32.Parse(l.zipcode))
            {
                latitude2 = Double.Parse(l.latitude);
                longitude2 = Double.Parse(l.longitude);
            }
        }

        //Haversine Forumla calcuates the distance between the latitude's and the longitude's of the 2 zipcodes
        double latitude1Rad = latitude1 * Math.PI / 180.0;
        double latitude2Rad = latitude2 * Math.PI / 180.0;

        double latDifferenceRad = (latitude2 - latitude1) * (Math.PI / 180);
        double lonDifferenceRad = (longitude2 - longitude1) * (Math.PI / 180);
        double a = Math.Pow(Math.Sin(latDifferenceRad / 2), 2) + (Math.Cos(latitude1Rad) * Math.Cos(latitude2Rad) * Math.Pow(Math.Sin(lonDifferenceRad / 2), 2));
        double c = 2 * Math.Asin(Math.Sqrt(a));
        double distance = 6371 * c;
        return distance;
    }
}
