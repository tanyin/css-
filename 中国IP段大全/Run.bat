echo off
step1.exe <ipcnt.txt > step2data.txt
step2.exe < step2data.txt > step3data.txt
step3.exe < step3data.txt > step4data.txt
step4.exe < step4data.txt > finaldata.txt
randomIP.exe < step4data.txt > testip.txt
