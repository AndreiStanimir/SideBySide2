const fs = require('fs');
const path = require('path');
const { execSync } = require('child_process');
const os = require('os');

// Paths to clear
const pathsToClear = [
  path.join(os.homedir(), 'AppData', 'Roaming', 'Electron'),
  path.join(os.homedir(), 'AppData', 'Roaming', 'side-by-side-translator'),
  path.join(os.homedir(), 'AppData', 'Local', 'Electron'),
  path.join(os.homedir(), 'AppData', 'Local', 'side-by-side-translator'),
];

// Kill Electron processes
try {
  console.log('Killing Electron processes...');
  execSync('taskkill /F /IM electron.exe /T', { stdio: 'ignore' });
} catch (e) {
  // Ignore if no processes to kill
}

// Clear directories
pathsToClear.forEach(dirPath => {
  try {
    console.log(`Clearing: ${dirPath}`);
    if (fs.existsSync(dirPath)) {
      fs.rmSync(dirPath, { recursive: true, force: true });
      console.log(`Successfully removed: ${dirPath}`);
    } else {
      console.log(`Directory does not exist: ${dirPath}`);
    }
  } catch (error) {
    console.error(`Error clearing ${dirPath}: ${error.message}`);
  }
});

console.log('\nCache cleared. You can now run the app again with:');
console.log('npm run dev'); 