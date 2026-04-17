const fs = require("fs");

// Đọc file
const content = fs.readFileSync("connected_people.txt", "utf-8");

// Tách từng dòng, bỏ dòng trống
const lines = content
  .split("\n")
  .map(line => line.trim())
  .filter(line => line.length > 0);

// Parse từng dòng thành object
const result = lines.map(line => {
  const [name, profileUrl, connectedAt] = line
    .split("|")
    .map(x => x.trim());

  return {
    name,
    profileUrl,
    connectedAt
  };
});

// Xuất ra file JSON
fs.writeFileSync(
  "connected_people.json",
  JSON.stringify(result, null, 2),
  "utf-8"
);

console.log("✅ Convert thành công -> connected_people.json");
