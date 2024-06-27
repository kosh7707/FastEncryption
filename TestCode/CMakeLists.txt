cmake_minimum_required(VERSION 3.26)
project(CPractice)

set(CMAKE_CXX_STANDARD 17)

# Create an executable
add_executable(CPractice main.cpp
                         AES.cpp
                         AES.h
        ARIA.cpp
        ARIA.h
        HIGHT.cpp
        HIGHT.h
        TWINE.cpp
        TWINE.h
        SPECK.cpp
        SPECK.h)

# include winsock2_32
target_link_libraries(CPractice PRIVATE ws2_32)

# include static stdc++ library for distribution
set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -static-libstdc++ -static-libgcc")

