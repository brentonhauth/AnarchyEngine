#version 330 core
// From OpenTK Tutorial
uniform vec3 uColor;
uniform sampler2D texture0;
uniform vec3 viewPos;
uniform vec3 lightPos;
uniform int uTextureCount;

out vec4 FragColor;

in vec3 Normal;
in vec3 FragPos;
in vec2 TexCoords;

void main() {
    float ambientStrength = 0.01;
	vec3 lightColor = vec3(1.0, 1.0, 1.0);
    vec3 ambient = ambientStrength * lightColor;

    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(lightPos - FragPos);

    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = diff * lightColor;

    float specularStrength = 0.5;
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
    vec3 specular = specularStrength * spec * lightColor;

	vec3 tex = uTextureCount == 0 ? vec3(1.0, 1.0, 1.0) : vec3(texture(texture0, TexCoords));

    vec3 result = (ambient + diffuse + specular) * tex;
	
    FragColor = vec4((result + uColor) * 0.5, 1.0);
}