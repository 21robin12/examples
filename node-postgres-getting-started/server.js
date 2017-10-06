var pg = require("pg");

// postgresql://<user>:<password>@<server>/<database>
var connectionString = "postgresql://postgres:password123@localhost/test";
var client = new pg.Client(connectionString);
client.connect();

client.query("select * from booking.day order by id", function(err, res) {
  if (err) {
    console.log(err.stack);
  } else {
	for(var i = 0; i < res.rows.length; i++) {
		console.log(res.rows[i]);
	}
  }
});

console.log("success");


