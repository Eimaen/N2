# N<sub>2</sub> <sub><sup>(Nitrogen)</sup></sub>
The proper Discord Nitro code generator and checker.

## <sub><sup>not</sup></sub> FAQ
*"not" just because c'mon why don't you pay for fxxing nitro?)*
### Why?
All the generators that I saw on github contained a bunch of errors in their code. I took everything into account and wrote my own. Because why not?

### How to use?

#### From built source
1. Download an archive from the [releases page](https://github.com/Eimaen/Nitrogen/releases).
2. Unzip and edit configuration in `Nitrogen.dll.config`.
3. Fill the file `proxies.txt` with proxies. Example:
```
socks4://192.168.13.37:4153
socks4://79.137.22.8:4153
http://1.1.1.1:1111
socks5://13.37.13.37:1337
```
4. Start the program, glhf.

#### Build your own
1. Build sources (restore NuGet packages).
2. Change config stored in the output folder (`Nitrogen.dll.config`).
3. Create a `proxies.txt` file in the output folder.
4. Fill the file `proxies.txt` with proxies. Example:
```
socks4://192.168.13.37:4153
socks4://79.137.22.8:4153
http://1.1.1.1:1111
socks5://13.37.13.37:1337
```
5. Start the program, glhf.
