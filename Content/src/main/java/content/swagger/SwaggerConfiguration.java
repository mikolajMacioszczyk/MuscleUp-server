package content.swagger;

import org.springdoc.core.GroupedOpenApi;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

//http://localhost:8080/swagger-ui/index.html
@Configuration
public class SwaggerConfiguration {

    @Bean
    public GroupedOpenApi publicApi() {

        return GroupedOpenApi.builder()
                .group("Test-controller")
                .packagesToScan("content.exampleController")
                .pathsToMatch("/test/*")
                .build();
    }
}
