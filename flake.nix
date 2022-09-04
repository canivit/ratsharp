{
  description = "ratsharp";

  inputs = { 
    nixpkgs.url = "github:nixos/nixpkgs/master";
  };

  outputs = { self, nixpkgs }: let
    pkgs = import nixpkgs {
      system = "x86_64-linux";
    };

    libPath = with pkgs; pkgs.lib.strings.makeLibraryPath [ 
      openssl 
      icu
    ];
  in {
    devShell.x86_64-linux = pkgs.mkShell {
      name = "othello";
      buildInputs = with pkgs; [
        dotnet-sdk_6
      ];
      shellHook = ''
        export DOTNET_ROOT=${pkgs.dotnet-sdk}
        export LD_LIBRARY_PATH=${libPath}
        dotnet tool restore
      '';
    };
  };
}
