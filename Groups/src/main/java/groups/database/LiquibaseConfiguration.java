package groups.database;

import liquibase.integration.spring.SpringLiquibase;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;
import org.springframework.core.env.Environment;
import org.springframework.util.Assert;

import javax.sql.DataSource;

public class LiquibaseConfiguration {

    private static final String LIQUIBASE_CHANGE_LOG_LOCATION = "spring.liquibase.change-log";

    private final DataSource dataSource;
    private final Environment env;


    @Autowired
    public LiquibaseConfiguration(Environment env, DataSource dataSource) {

        Assert.notNull(dataSource, "dataSource must not be null");
        Assert.notNull(env, "env must not be null");

        this.dataSource = dataSource;
        this.env = env;
    }


    @Bean
    public SpringLiquibase liquibase() {

        SpringLiquibase liquibase = new SpringLiquibase();

        liquibase.setChangeLog(env.getProperty(LIQUIBASE_CHANGE_LOG_LOCATION));
        liquibase.setDataSource(dataSource);

        return liquibase;
    }
}
