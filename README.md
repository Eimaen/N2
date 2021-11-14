# N<sub>2</sub> <sub><sup>(Nitrogen)</sup></sub>
The proper Discord Nitro code generator and checker.

## <sub><sup>not</sup></sub> FAQ
*"not" just because c'mon why don't you pay for fxxing nitro?)*
### Why?
All the generators that I saw on github contained a bunch of errors in their code. I took everything into account and wrote my own. Because why not?

### How to use?
1. Change webhook URL in `Program.cs` file (bottom). You can also change thread count and file names.
2. Build sources (restore NuGet packages).
3. Go to the output folder and create a `proxies.txt` file.
4. Fill the file `proxies.txt` with proxies. Example:
```
socks4://192.168.13.37:4153
socks4://79.137.22.8:4153
http://1.1.1.1:1111
socks5://13.37.13.37:1337
```
5. Start the program and wait.
