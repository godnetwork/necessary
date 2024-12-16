const { app, BrowserWindow, ipcMain } = require('electron');
const { writeFileSync, readFileSync } = require('fs');
const { execSync } = require('child_process');
const { app, BrowserWindow, ipcMain } = require('electron');
const path = require('path');
const { execSync } = require('child_process');
const fs = require('fs');
const { contextBridge, ipcRenderer } = require('electron');

contextBridge.exposeInMainWorld('electronAPI', {
    processAction: (action, data) => ipcRenderer.invoke('process-action', action, data),
});
function createWindow() {
    mainWindow = new BrowserWindow({
        width: 800,
        height: 600,
        webPreferences: {
            preload: path.join(__dirname, 'preload.js'),
            nodeIntegration: false,
            contextIsolation: true,
        }
    });

    mainWindow.loadFile('index.html');
}

app.whenReady().then(() => {
    createWindow();

    app.on('activate', () => {
        if (BrowserWindow.getAllWindows().length === 0) createWindow();
    });
});

app.on('window-all-closed', () => {
    if (process.platform !== 'darwin') app.quit();
});

// Handle IPC requests
ipcMain.handle('process-action', async (_, action, data) => {
    const requestFile = path.join(__dirname, 'request.json');
    const responseFile = path.join(__dirname, 'response.json');

    fs.writeFileSync(requestFile, JSON.stringify({ action, ...data }, null, 2));

    try {
        // Execute VB.NET middleware
        execSync(`"${path.join(__dirname, 'Middleware', 'AttendanceApp.exe')}" "${requestFile}" "${responseFile}"`);

        const response = fs.readFileSync(responseFile, 'utf-8');
        return { success: true, message: response };
    } catch (error) {
        console.error('Error processing action:', error);
        return { success: false, message: 'An error occurred.' };
    }
});

let mainWindow;

app.whenReady().then(() => {
    mainWindow = new BrowserWindow({
        width: 800,
        height: 600,
        webPreferences: {
            nodeIntegration: true
        }
    });
    mainWindow.loadFile('index.html');
});

ipcMain.on('request', (event, request) => {
    writeFileSync('request.json', JSON.stringify(request));
    execSync('AttendanceApp.exe request.json response.json');
    const response = readFileSync('response.json', 'utf8');
    event.reply('response', response);
});
