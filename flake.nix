{
  description = "ratsharp";

  outputs = { self, nixpkgs }: let
    pkgs = import nixpkgs {
      system = "x86_64-linux";
    };

    libPath = pkgs.lib.strings.makeLibraryPath [ pkgs.openssl ];
  in {
    devShell.x86_64-linux = pkgs.mkShell {
      name = "othello";
      buildInputs = with pkgs; [
        dotnet-sdk_6
      ];
      shellHook = ''
        export LD_LIBRARY_PATH=${libPath}
      '';
    };
  };
}
